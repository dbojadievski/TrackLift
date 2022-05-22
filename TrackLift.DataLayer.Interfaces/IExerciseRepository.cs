using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TrackLift.Models;

namespace TrackLift.DataLayer.Interfaces
{
    public interface IExerciseRepository: IRepository<Exercise>
    {
        IEnumerable<Exercise> GetByType(ExerciseType exerciseType);
        IEnumerable<Exercise> GetByImprovementTarget(ExerciseTarget exerciseTarget);
        IEnumerable<Exercise> GetByMuscleGroup(MuscleGroup muscleGroup);
    }
}
