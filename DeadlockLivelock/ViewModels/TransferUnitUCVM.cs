using DeadlockLivelock.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using DeadlockLivelock.Models;
using System.Collections.ObjectModel;
using DeadlockLivelock.Views;

namespace DeadlockLivelock.ViewModels
{
    public class TransferUnitUCVM : INotifyPropertyChanged
    {
        private ObservableCollection<TransferUnitUC> _transferUnitsUC;
        private TransferUnit _tu;
        private List<TransferUnit> _transferUnitsList;

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


        public TransferUnitUCVM(
            TransferUnit tu,
            ObservableCollection<TransferUnitUC> transferUnitsUC,
            List<TransferUnit> transferUnitsList
        )
        {
            _transferUnitsUC = transferUnitsUC;
            _transferUnitsList = transferUnitsList;
            _tu = tu;

            DeleteTUCommand = new RelayCommand(DeleteTU);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DeleteTU (object self) {

            if(TU.Status == TransferStatus.TRANSFERING)
            {
                MessageBox.Show(
                    "Не може да се изтрие заявка за трансфер, която е започнала.", 
                    "Невалидна операция",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return;
            }

            // Removing the controll from the list and from the UI
            _transferUnitsUC.Remove(self as TransferUnitUC);
            _transferUnitsList.Remove(TU);
        }

    }
}
