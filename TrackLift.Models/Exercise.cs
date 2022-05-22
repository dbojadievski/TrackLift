using System;
using System.Collections.Generic;
using System.Text;

namespace TrackLift.Models
{
    public enum ExerciseType
    {
        Competition,
        SPP,
        GPP
    }

    public enum ExerciseTarget
    {
        Unknown,
        Squat,
        Bench,
        Deadlift
    }

    public enum MuscleGroup
    {
        Unknown,
        Traps,
        UpperBack,
        Delts,
        MiddleBack,
        Lats,
        LowerBack,
        Abs,
        Hamstring,
        Glutes,
        Calves,
        Quads,
        Hamstrings,
        Triceps,
        Biceps,
        Chest
    }

    public enum ExerciseForceType
    {
        Unknown,
        Push,
        Pull,
        Isometric
    }

    public enum ExerciseMechanics
    {
        Unknown,
        Compound,
        Isolation
    }

    public class Exercise : ModelBase
    {
        private String name;
        private ExerciseType type;
        private ExerciseTarget target;
        private MuscleGroup mainMuscleGroup;
        private ExerciseMechanics mechanics;
        private ExerciseForceType forceType;
        private TimeSpan restTime;
        private String? note;
        private String? instructionVideo;

        public String Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public String? Note
        {
            get => note;
            set => SetProperty(ref note, value);
        }

        public ExerciseType Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        public ExerciseTarget Target
        {
            get => target;
            set => SetProperty(ref target, value);
        }

        public MuscleGroup MainMuscleGroup
        {
            get => mainMuscleGroup;
            set => SetProperty(ref mainMuscleGroup, value);
        }

        public ExerciseMechanics Mechanics
        {
            get => mechanics;
            set => SetProperty(ref mechanics, value);
        }

        public ExerciseForceType ForceType
        {
            get => forceType;
            set => SetProperty(ref forceType, value);
        }

        public TimeSpan RestTime
        {
            get => restTime;
            set => SetProperty(ref restTime, value);
        }

        public String? InstructionVideo
        {
            get => instructionVideo;
            set => SetProperty(ref instructionVideo, value);
        }
    }
}
