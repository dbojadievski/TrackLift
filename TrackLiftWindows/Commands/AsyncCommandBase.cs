using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrackLiftWindows.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public event EventHandler<bool> CommandExecuted;

        public abstract bool CanExecute(object? parameter);

        public virtual async void Execute(object? parameter)
        {
            await Task.Run(() => ExecuteCommandAsync(parameter));
        }

        private async Task ExecuteCommandAsync(object? parameter)
        {
            bool result = await DoWork(parameter);
            if (CommandExecuted != null)
            {
                CommandExecuted(this, result);
            }

        }

        protected virtual async Task<bool> DoWork(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
