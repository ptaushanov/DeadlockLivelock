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
    public class TransferUnitUCVM : INotifyPropertyChanged
    {
        private ObservableCollection<TransferUnitUC> _transferUnitsUC;
        private TransferUnit _tu;

        public TransferUnit TU
        {
            get { return _tu; }
            set
            {
                _tu = value;
                OnPropertyChanged("TU");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand DeleteTUCommand { get; private set; }


        public TransferUnitUCVM(TransferUnit tu, ObservableCollection<TransferUnitUC> transferUnitsUC)
        {
            _transferUnitsUC = transferUnitsUC;
            _tu = tu;

            DeleteTUCommand = new RelayCommand(DeleteTU);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DeleteTU (object self) {

            // Removing the controll from the list and from the UI
            _transferUnitsUC.Remove(self as TransferUnitUC);
        }

    }
}
