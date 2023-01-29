using System;
using System.Threading.Tasks;
using System.Windows.Input;


namespace DeadlockLivelock.Utils
{
    public class RelayCommandAsync : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Func<object, Task> action;

        public bool CanExecute(object parameter) { return true; }

        public RelayCommandAsync(Func<object, Task> execute)
        {
            action = execute;
        }

        public async void Execute(object parameter)
        {
            await action.Invoke(parameter);
        }
    }
}
