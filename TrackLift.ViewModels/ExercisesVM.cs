using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using TrackLift.DataLayer.Interfaces;
using TrackLift.Models;
using TrackLift.ViewModels.Commands;

using static System.Diagnostics.Debug;
namespace TrackLift.ViewModels
{
    public class ExercisesVM : ViewModelBase
    {
        #region Public properties.
        public ObservableCollection<Exercise> Exercises
        {
            get => exercises;
            set => SetProperty(ref exercises, value);
        }
        public ICommand ParseSheikoGoldCSVCommand
        {
            get => parseSheikoGoldCSVCommand;
            private set => SetProperty(ref parseSheikoGoldCSVCommand, value);
        }

        public ICommand DeleteExerciseDBCommand
        {
            get => deleteExerciseDBCommand;
            private set => SetProperty(ref deleteExerciseDBCommand, value);
        }
        #endregion

        #region Public functions.
        public void AddExercise(Exercise ex)
        {
            Assert(ex != null);
            exercises.Add(ex);
        }
        #endregion
        
        #region Constructors.
        public ExercisesVM(IExerciseRepository exerciseRepository, ILogger logger)
        {
            this.exerciseRepository = exerciseRepository;
            this.logger = logger;

            ParseSheikoGoldCSVCommand = new ParseSheikoGoldCSVCommand(exerciseRepository, logger);
            DeleteExerciseDBCommand = new DeleteExerciseDBCommand(exerciseRepository, logger);

            Exercises = new ObservableCollection<Exercise>(exerciseRepository.GetAll());
        }

        #endregion

        #region Private variables.
        private ObservableCollection<Exercise> exercises;
        private IExerciseRepository exerciseRepository;

        private ILogger logger;

        private ICommand parseSheikoGoldCSVCommand;
        private ICommand deleteExerciseDBCommand;
        #endregion
    }
}
