using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadlockLivelock.Models;
using DeadlockLivelock.Utils;
using DeadlockLivelock.Views;

namespace DeadlockLivelock.ViewModels
{
    public class TransferWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TransferManager> BankAccounts { get; private set; }

        private ObservableCollection<TransferUnitUC> _transferUnitUCList;
        private List<TransferUnit> _transferUnitsList;
        private TransferWindow _transferWindow;

        private TransferManager _transferFrom;
        private TransferManager _transferTo;
        private double _transferAmount;

        public RelayCommand CreateTransferCommand { get; private set; }

        public TransferManager TransferFrom
        {
            get { return _transferFrom; }
            set
            {
                _transferFrom = value;
                OnPropertyChanged("TransferFrom");
            }
        }

        public TransferManager TransferTo
        {
            get { return _transferTo; }
            set
            {
                _transferTo = value;
                OnPropertyChanged("TransferTo");
            }
        }

        public double TransferAmount
        {
            get { return _transferAmount; }
            set
            {
                _transferAmount = value;
                OnPropertyChanged("TransferAmount");
            }
        }

        public TransferWindowVM(
            ObservableCollection<TransferUnitUC> transferUnitsUC,
            ObservableCollection<TransferManager> bankAccounts,
            List<TransferUnit> transferUnitsList,
            TransferWindow transferWindow
        )
        {
            _transferUnitUCList = transferUnitsUC;
            BankAccounts = bankAccounts;
            _transferUnitsList = transferUnitsList;
            _transferWindow = transferWindow;

            CreateTransferCommand = new RelayCommand(CreateTransfer);
        }

        private void CreateTransfer(object _)
        {
            TransferUnit newTU = new TransferUnit(TransferFrom, TransferTo, TransferAmount);


            TransferUnitUC newTUUC = new TransferUnitUC()
            {
                DataContext = new TransferUnitUCVM(newTU, _transferUnitUCList)
            };

            _transferUnitsList.Add(newTU);
            _transferUnitUCList.Add(newTUUC);
            Debug.WriteLine(_transferUnitUCList.Count);
            _transferWindow.Close();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
