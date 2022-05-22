using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using TrackLift.DataLayer.Interfaces;
using TrackLift.Models;

using TrackLiftWindows.Commands;

using static System.Diagnostics.Debug;
namespace TrackLift.ViewModels.Commands
{
    public class ParseSheikoGoldCSVCommand : AsyncCommandBase
    {
        #region Public functions.
        public override bool CanExecute(object? parameter)
        {
            return true;
        }
        #endregion

        #region Constructors.
        public ParseSheikoGoldCSVCommand(IExerciseRepository exerciseRepository, ILogger logger)
        {
            this.exerciseRepository = exerciseRepository;
            this.logger = logger;
        }
        #endregion

        #region Private properties.
        private static string ExerciseFieldName
        {
            get => "Exercise";
        }

        private static string MuscleGroupFieldName
        {
            get => "Main_Muscle";
        }

        private static string ForceTypeFieldName
        {
            get => "Force";
        }
        #endregion
        
        #region Private methods.
        private async Task<bool> ParseSheikoGoldCSV(string filePath)
        {
            bool result = false;

            List<Exercise> parsedExercises = new List<Exercise>();

            Dictionary<int, string> csvFieldsByOrder = new Dictionary<int, string>();
            Dictionary<string, int> csvFieldsByName = new Dictionary<string, int>();
            HashSet<string> knownExercisesSet = new HashSet<string>();
            var allExercises = exerciseRepository.GetAll();
            foreach (var exercise in allExercises)
            {
                knownExercisesSet.Add(exercise.Name);
            }

            using (var reader = File.OpenText(filePath))
            {
                if (!reader.EndOfStream)
                {
                    // Consume the first line and build the fields list.
                    string? line = await reader.ReadLineAsync();
                    if (line == null)
                        return false;

                    var tokens = ParseSheikoGoldLine(line);

                    for (int idxToken = 0; idxToken < tokens.Count; idxToken++)
                    {

                        string token = tokens[idxToken].Trim();
                        token = token.Replace("\"", "");
                        csvFieldsByOrder.Add(idxToken, token);
                        csvFieldsByName.Add(token, idxToken);
                    }
                }

                // Parse each line.
                while (!reader.EndOfStream)
                {

                    Exercise exercise = new Exercise();

                    string? line = await reader.ReadLineAsync();
                    
                    if (line == null)
                        break;

                    try
                    {
                        var tokens = ParseSheikoGoldLine(line);

                        string name = GetTokenByName(ExerciseFieldName, tokens, csvFieldsByName);
                        bool alreadyExists = knownExercisesSet.Contains(name);
                        if (alreadyExists)
                            continue;
                        else
                            knownExercisesSet.Add(name);
                        exercise.Name = name;

                        exercise.MainMuscleGroup = ParseSheikoGoldMuscleGroup(GetTokenByName(MuscleGroupFieldName, tokens, csvFieldsByName));
                        exercise.Mechanics = ParseSheikoGoldMechanics(GetTokenByName(MuscleGroupFieldName, tokens, csvFieldsByName));
                        exercise.ForceType = ParseSheikoGoldForceType(GetTokenByName(ForceTypeFieldName, tokens, csvFieldsByName));
                        exercise.Type = ParseSheikoGoldExerciseType(exercise.Name);
                        exercise.Target = ParseSheikoGoldExerciseTarget(exercise);

                        exercise.CreatedAt = DateTime.Now;
                        exercise.UpdatedAt = DateTime.Now;
                        parsedExercises.Add(exercise);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Error parsing Sheiko Gold file: {ex.Message}");
                    }
                }

                if (parsedExercises.Count > 0)
                    result = exerciseRepository.Insert(parsedExercises);
                else
                    result = true;
                Assert(result, "Failed inserting exercises parsed from SG .csv file.");
            }

            return result;
        }

        private string GetTokenByName(string tokenName, IEnumerable<string> tokens, Dictionary<string, int> fieldsByName)
        {
            try
            {
                var idx = fieldsByName[tokenName];

                var token = tokens.ElementAt(idx);
                var trimmed = token.Trim().ToLowerInvariant().Replace("\"", "");
                return trimmed;
            }
            catch
            {
                return String.Empty;
            }
        }

        private bool IsDumbbellExercise(string name)
        {
            return name.Contains("dumbbell") 
                || name.Contains("db")
            ;
        }

        private bool IsKettlebellExercise(string name)
        {
            return name.Contains("kettlebell") 
                || name.Contains("kb")
                || name.Contains("goblet")
                || name.Contains("goblin")
            ;
        }

        private bool IsBodyweightExercise(string name)
        {
            return name.Contains("bodyweight") 
                || name.Contains("body weight") 
                || name.Contains("bw") 
                || name.Contains("weighted")
            ;
        }

        private bool IsZercherExercise(string name)
        {
            return name.Contains("zercher");
        }

        private bool IsJeffersonExercise(string name)
        {
            return name.Contains("jefferson");
        }

        private bool IsSmithMachineExercise(string name)
        {
            return name.Contains("smith");
        }

        private bool IsCableMachineExercise(string name)
        {
            return name.Contains("cable");
        }

        private bool IsSquatVariation(string name)
        {
            bool hasSquatInName = name.Contains("squat");
            bool isSissy = name.Contains("sissy");
            bool isSplit = name.Contains("split");

            return hasSquatInName && !isSissy && !isSplit;
        }

        private bool IsDeadliftVariation(string name)
        {
            bool hasDeadliftInName = name.Contains("deadlift") || name.Contains("pull"); // block pull, rack pull...

            bool isRdl = name.Contains("romanian") || name.Contains("rdl");
            bool isStiffLegged = name.Contains("stiff") || name.Contains("sldl");

            return name.Contains("deadlift");
        }

        private bool IsBenchPressVariation(string name)
        {
            bool hasBenchInName = name.Contains("bench") || name.Contains("bench press") || name.Contains("press"); // Think floor-press, board-press, speed bench.
            
            bool isIncline = name.Contains("incline");
            bool isDecline = name.Contains("decline");

            bool isOverhead = name.Contains("standing") || name.Contains("seated") || name.Contains("military") || name.Contains("overhead");

            return hasBenchInName && !isIncline && !isDecline && !isOverhead;
        }

        private ExerciseTarget ParseSheikoGoldExerciseTarget(Exercise exercise)
        {
            ExerciseTarget target = ExerciseTarget.Unknown;

            if (IsSquatVariation(exercise.Name))
                target = ExerciseTarget.Squat;
            else if (IsBenchPressVariation(exercise.Name))
                target = ExerciseTarget.Bench;
            else if (IsDeadliftVariation(exercise.Name))
                target = ExerciseTarget.Deadlift;
            else switch (exercise.MainMuscleGroup)
            {
                    case MuscleGroup.Quads:
                    case MuscleGroup.Calves:
                    case MuscleGroup.Abs:
                    case MuscleGroup.UpperBack:
                    case MuscleGroup.Biceps:
                        target = ExerciseTarget.Squat;
                        break;
                    case MuscleGroup.Glutes:
                    case MuscleGroup.Hamstring:
                    case MuscleGroup.LowerBack:
                    case MuscleGroup.Lats:
                        target = ExerciseTarget.Deadlift;
                        break;
                    case MuscleGroup.Chest:
                    case MuscleGroup.Delts:
                    case MuscleGroup.Triceps:
                    case MuscleGroup.Traps:
                    case MuscleGroup.MiddleBack:
                        target = ExerciseTarget.Bench;
                        break;
            }
            return target;
        }

        private ExerciseType ParseSheikoGoldExerciseType(string name)
        {
            ExerciseType exerciseType = ExerciseType.GPP;

            name = name.Trim().ToLowerInvariant();
            
            if (name.StartsWith("competition"))
                exerciseType = ExerciseType.Competition;
            else
            {
                if (   
                       IsDumbbellExercise(name) 
                    || IsKettlebellExercise(name) 
                    || IsBodyweightExercise(name) 
                    || IsSmithMachineExercise(name) 
                    || IsCableMachineExercise(name)
                    || IsZercherExercise(name)
                    || IsJeffersonExercise(name)
                )
                    exerciseType = ExerciseType.GPP;
                else if (IsSquatVariation(name) || IsBenchPressVariation(name) || IsDeadliftVariation(name))
                    exerciseType = ExerciseType.SPP;
            }

            return exerciseType;
        }

        private ExerciseForceType ParseSheikoGoldForceType(string input)
        {
            switch (input)
            {
                case "push":
                    return ExerciseForceType.Push;
                case "pull":
                    return ExerciseForceType.Pull;
                case "isometric":
                    return ExerciseForceType.Isometric;
                default:
                {
                    //Assert(input == String.Empty, $"Unsupported exercise force type {input}");
                    return ExerciseForceType.Unknown;
                }
            }
        }

        private ExerciseMechanics ParseSheikoGoldMechanics(string input)
        {
            switch (input)
            {
                case "compound":
                    return ExerciseMechanics.Compound;
                case "isolation":
                    return ExerciseMechanics.Isolation;
                default:
                    return ExerciseMechanics.Unknown;
            }
        }

        private MuscleGroup ParseSheikoGoldMuscleGroup(string input)
        {
            switch (input)
            {
                case "chest":
                    return MuscleGroup.Chest;
                case "shoulders":
                    return MuscleGroup.Delts;
                case "glutes":
                    return MuscleGroup.Glutes;
                case "hamstrings":
                    return MuscleGroup.Hamstring;
                case "quadriceps":
                    return MuscleGroup.Quads;
                case "calves":
                    return MuscleGroup.Calves;
                case "traps":
                    return MuscleGroup.Traps;
                case "lats":
                    return MuscleGroup.Lats;
                case "middle back":
                    return MuscleGroup.MiddleBack;
                case "upper back":
                    return MuscleGroup.UpperBack;
                case "lower back":
                    return MuscleGroup.LowerBack;
                case "abdominals":
                    return MuscleGroup.Abs;
                case "triceps":
                    return MuscleGroup.Triceps;
                case "biceps":
                    return MuscleGroup.Biceps;
                default:
                    //Assert(input ==String.Empty, $"Unsupported muscle group {input} detected.");
                    return MuscleGroup.Unknown;
            }
        }

        private List<string> ParseSheikoGoldLine(string line)
        {
            bool isInToken = false;
            int idxStart = 0;
            List<string> tokens = new List<string>();

            for (int currIdx = 0; currIdx < line.Length; currIdx++)
            {
                if (line[currIdx] == '"')
                {
                    if (isInToken)
                    {
                        // Create token.
                        isInToken = false;
                        string token = line.Substring(idxStart, currIdx - idxStart);
                        tokens.Add(token);
                    }
                    else
                    {
                        // start token.
                        isInToken = true;
                        idxStart = currIdx + 1;
                    }
                }
            }

            if (isInToken)
            {
                string token = line.Substring(idxStart);
                tokens.Add(token);
            }


            return tokens;
        }

        protected override async Task<bool> DoWork(object? parameter)
        {
            bool result = false;
            parameter = "C:\\Users\\dboja\\OneDrive\\Desktop\\SheikoCSV.csv";
            string? filePath = parameter as string;
            Assert(filePath != null);

            if (filePath == null)
            {
                logger.LogError("Attempting to execute command ParseSheikoGoldCommandName with invalid parameter. The command cannot proceed.");
            }
            else
            {
                result = await ParseSheikoGoldCSV(filePath);
            }

            return result;
        }
        #endregion

        #region Public variables.
        public event EventHandler? CanExecuteChanged;
        #endregion

        #region Private variables.
        private IExerciseRepository exerciseRepository;
        private ILogger logger;
        #endregion
    }
}
