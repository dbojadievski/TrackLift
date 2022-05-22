using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using TrackLift.DataLayer.Interfaces;

using TrackLiftWindows.Commands;

namespace TrackLift.ViewModels.Commands
{
    public class DeleteExerciseDBCommand : AsyncCommandBase
    {
        #region Public functions.
        public override bool CanExecute(object? parameter)
        {
            return true;
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
        protected override async Task<bool> DoWork(object? parameter)
        {
            bool didDelete = this.exerciseRepository.DeleteAll();
            return didDelete;
        }
        #endregion

        #region Private variables.
        private IExerciseRepository exerciseRepository;
        private ILogger logger;
        #endregion
    }
}
