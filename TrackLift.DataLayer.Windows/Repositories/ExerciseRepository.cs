using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;

using Microsoft.Extensions.Logging;

using TrackLift.DataLayer.Interfaces;
using TrackLift.Models;

using static System.Diagnostics.Debug;

namespace TrackLift.DataLayer.Windows.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        #region Properties.
        public string TableName => "Exercise";
        #endregion

        #region Constructors.
        public ExerciseRepository(ILogger logger)
        {
            Assert(logger != null, "Cannot construct class, as required variable 'logger' of type 'Ilogger' is missing.");
            this.logger = logger;

            if (!DoesTableExist())
            {
                CreateTable();
            }
        }

        #endregion
        #region Public methods.
        public bool CreateTable()
        {
            bool result = false;
            
            try
            {
                int numAffected = SQLiteProvider.Database.CreateTable<Exercise>();
                result = (numAffected > 0);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error creating table {TableName}: {ex.Message}");
            }
            return result;
        }

        public bool Delete(IEnumerable<Exercise> entries)
        {
            bool result = false;

            try
            {
                string sql = $"DELETE FROM {TableName} e WHERE e.id = @Id";
                int numAffected = SQLiteProvider.Database.Execute(sql, entries);
                result = (numAffected > 0);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting entries for table Exercise: {ex.Message}");
            }

            return result;
        }

        public bool DeleteAll()
        {
            bool result = false;

            try
            {
                int numAffected = SQLiteProvider.Database.DeleteAll<Exercise>();
                result = (numAffected > 0);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting entries for table {TableName}: {ex.Message}");
            }

            return result;

        }

        public bool DeleteTable()
        {
            bool result = false;

            try
            {
                int numAffected = SQLiteProvider.Database.Execute($"DROP TABLE IF EXISTS {TableName}");
                result = (numAffected > 0);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error dropping table {TableName}: {ex.Message}");
            }

            return result;
        }

        public bool DoesTableExist()
        {
            return SQLiteProvider.DoesTableExist(TableName);
        }

        public IEnumerable<Exercise> GetAll()
        {
            IEnumerable<Exercise> result = null;

            try
            {
                string sql = $"SELECT * FROM {TableName} ORDER BY Name";
                result = SQLiteProvider.Database.Query<Exercise>(sql);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error selecting entries for table Exercise: {ex.Message}");
            }

            return result;
        }

        public IEnumerable<Exercise> GetByImprovementTarget(ExerciseTarget exerciseTarget)
        {
            IEnumerable<Exercise> result = null;

            try
            {
                string sql = $"SELECT * FROM {TableName} e WHERE e.Target = {exerciseTarget} ORDER BY Name";
                result = SQLiteProvider.Database.Query<Exercise>(sql);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error selecting entities for table Exercise: {ex.Message}");
            }

            return result;
        }

        public IEnumerable<Exercise> GetByMuscleGroup(MuscleGroup muscleGroup)
        {
            IEnumerable<Exercise> result = null;

            try
            {
                string sql = $"SELECT * FROM {TableName} e WHERE e.MainMuscleGroup = {muscleGroup} ORDER BY Name";
                result = SQLiteProvider.Database.Query<Exercise>(sql);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error selecting entities for table Exercise: {ex.Message}");
            }

            return result;
        }

        public IEnumerable<Exercise> GetByType(ExerciseType exerciseType)
        {
            IEnumerable<Exercise> result = null;

            try
            {
                string sql = $"SELECT * FROM {TableName} e WHERE e.type = {exerciseType} ORDER BY Name";
                result = SQLiteProvider.Database.Query<Exercise>(sql);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error selecting entities for table Exercise: {ex.Message}");
            }

            return result;
        }

        public bool Insert(IEnumerable<Exercise> entries)
        {
            bool result = false;
            try
            {
                // As the InsertAll runs in a transaction by default, the final inserted row count cannot be a number less than entries.Count anyway.
                int numAffected = SQLiteProvider.Database.InsertAll(entries);
                result = (numAffected > 0); 
            }
            catch (Exception ex)
            {
                logger.LogError($"Error inserting entities for table Exercise: {ex.Message}");
            }

            return result;
        }

        public bool Update(IEnumerable<Exercise> entries)
        {
            bool result = false;

            try
            {
                // As the UpdateAll runs in a transaction by default, the final inserted row count cannot be a number less than entries.Count anyway.
                int numAffected = SQLiteProvider.Database.UpdateAll(entries);
                result = (numAffected > 0);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error inserting entities for table Exercise: {ex.Message}");
            }

            return result;
        }
        #endregion

        #region Private variables.
        ILogger logger;
        #endregion
    }
}
