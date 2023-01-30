using DeadlockLivelock.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.ViewModels
{
    public class LogVM : INotifyPropertyChanged
    {
        private string _currentLog;

        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentLog
        {
            get { return _currentLog; }
            set { 
                _currentLog = value;
                OnPropertyChanged("CurrentLog");
            }
        }

        public RelayCommand RefreshLogCommand { get; private set; }
        public RelayCommand DeleteLogCommand { get; private set; }


        public LogVM()
        {
            CurrentLog = Logger.CurrentLogs;
            RefreshLogCommand = new RelayCommand(RefreshLog);
            DeleteLogCommand = new RelayCommand(DeleteLog);
        }


        private void RefreshLog(object _) => CurrentLog = Logger.CurrentLogs;
        private void DeleteLog(object _)
        {
            CurrentLog = string.Empty;
            Logger.DeleteLogs();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
