using System;
using System.Windows.Input;


namespace DeadlockLivelock.Utils
{
        public class RelayCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            private readonly Action<object> action;

            public bool CanExecute(object parameter) { return true; }

            public RelayCommand(Action<object> execute)
            {
                action = execute;
            }

            public void Execute(object parameter)
            {
                action.Invoke(parameter);
            }
    }
}
