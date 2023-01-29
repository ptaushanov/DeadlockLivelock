using DeadlockLivelock.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DeadlockLivelock.Models;
using System.Collections.ObjectModel;
using DeadlockLivelock.Views;

namespace DeadlockLivelock.ViewModels
{
    public class ComputeUnitUCVM : INotifyPropertyChanged
    {
        private string _cuName;
        private string _cuStatus;

        private readonly MainVM _mainVM;
        private readonly ComputeUnit _cu;

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommandAsync DeleteCUCommand { get; private set; }
        public RelayCommandAsync ComputeCommand {  get; private set; }

        public string CUName { get { return _cuName; }
            set { 
                _cuName = value;
                _cu.Name = value;
                OnPropertyChanged("CUName");
            }
        }

        public string CUStatus
        {
            get { return _cuStatus; }
            set
            {
                _cuStatus = value;
                _cu.Status = value;
                OnPropertyChanged("CUStatus");
            }
        }

        public ComputeUnitUCVM(ComputeUnit cu, MainVM mainVM)
        {
            this._mainVM = mainVM;
            _cu = cu;

            CUName = _cu.Name;
            CUStatus = _cu.Status;
            DeleteCUCommand = new RelayCommandAsync(DeleteCU);
            ComputeCommand = new RelayCommandAsync(Compute);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task DeleteCU (object self) {
            while (_mainVM.PendingOperations)
            {
                CUStatus = "Waiting for a pending operation ...";
                await Task.Delay(1000);
            }

            _mainVM.PendingOperations = true;
            await Task.Delay(2000);

            // Removing the controll from the list and from the UI
            var computeUnitList = _mainVM.ComputeUnitList;
            computeUnitList.Remove(self as ComputeUnitUC);

            // TODO: remove data from db
            _mainVM.PendingOperations = false;
        }

        private async Task Compute(object _)
        {
            while (_mainVM.PendingOperations)
            {
                CUStatus = "Waiting for a pending operation ...";
                await Task.Delay(1000);
            }

            CUStatus = "Work starting...";
        }

    }
}
