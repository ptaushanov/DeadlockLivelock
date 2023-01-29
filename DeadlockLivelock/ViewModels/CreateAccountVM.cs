using DeadlockLivelock.Models;
using DeadlockLivelock.Utils;
using DeadlockLivelock.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.ViewModels
{
    public class CreateAccountVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<TransferManager> _transferManagerList;
        private CreateAccountWindow _createAccountWindow;
        private string _accountName;
        private double _balance;

        public string AccountName
        {
            get { return _accountName; }
            set
            {
                _accountName = value;
                OnPropertyChanged("AccountName");
                OnPropertyChanged("CanCreateAccount");
            }
        }

        public double Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChanged("Balance");
            }
        }

        public bool CanCreateAccount
        {
            get
            {
                if(string.IsNullOrEmpty(_accountName)) return false;
                return true;
            }
        }

        public RelayCommand CreateAccountCommand { get; private set; }

        public CreateAccountVM(
            ObservableCollection<TransferManager> transferManagerList,
            CreateAccountWindow createAccountWindow
            )
        {
            _transferManagerList = transferManagerList;
            _createAccountWindow = createAccountWindow;
            CreateAccountCommand = new RelayCommand(CreateAccount);
        }

        private void CreateAccount(object _)
        {
            BankAccount newAccount = new BankAccount(null, Balance, AccountName);
            TransferManager newTransferManager = new TransferManager(newAccount);
            _transferManagerList.Add(newTransferManager);
            _createAccountWindow.Close();

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
