using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using TrackLift.DataLayer.Interfaces;

namespace TrackLift.ViewModels.Commands
{
    public class DeleteExerciseDBCommand : ICommand
    {
        #region Public properties.
        public event EventHandler? CanExecuteChanged;
        #endregion

        #region Public functions.
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            DeleteAllExercises();
        }
        #endregion

        #region Constructors.
        public DeleteExerciseDBCommand(IExerciseRepository exerciseRepository, ILogger logger)
        {
            this.exerciseRepository = exerciseRepository;
            this.logger = logger;
        }
        #endregion

        #region Private functions.
        private async void DeleteAllExercises()
        {
            this.exerciseRepository.DeleteAll();
        }
        #endregion

        #region Private variables.
        private IExerciseRepository exerciseRepository;
        private ILogger logger;
        #endregion
    }
}
