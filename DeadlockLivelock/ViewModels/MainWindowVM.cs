using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DeadlockLivelock.Models;
using DeadlockLivelock.Utils;
using DeadlockLivelock.Views;

namespace DeadlockLivelock.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private CancellationTokenSource _cancellationSource;
        public RelayCommand CreateNewTransferCommand { get; private set; }
        public RelayCommandAsync StartTransferCommand { get; private set; }
        public RelayCommand CreateAccountCommand { get; private set; }
        public RelayCommand OpenLogCommand { get; private set; }
        public RelayCommand ForceStopCommand { get; private set; }
        public List<TransferUnit> TransferUnitList { get; private set; }
        public ObservableCollection<TransferUnitUC> TransferUnitUCList { get; private set; }
        public ObservableCollection<TransferManager> TransferManagerList { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isDeadLockable;
        private bool _isLiveLockable;


        public bool IsDeadLockable
        {
            get { return _isDeadLockable; }
            set
            {
                _isDeadLockable = value;
                OnPropertyChanged("IsDeadLockable");
            }
        }

        public bool IsLiveLockable
        {
            get { return _isLiveLockable; }
            set
            {
                _isLiveLockable = value;
                OnPropertyChanged("IsLiveLockable");
            }
        }

        public MainWindowVM()
        {
            TransferManager studentAccount = new TransferManager(new BankAccount(1, 50000, "Student account"));
            TransferManager universityAccount = new TransferManager(new BankAccount(2, 100000, "University account"));

            TransferUnitUCList = new ObservableCollection<TransferUnitUC>();
            TransferManagerList = new ObservableCollection<TransferManager>()
            {
                studentAccount, universityAccount
            };
            TransferUnitList = new List<TransferUnit>();
            CreateNewTransferCommand = new RelayCommand(CreateNewTransfer);
            StartTransferCommand = new RelayCommandAsync(StartTransfer);
            CreateAccountCommand = new RelayCommand(CreateAccount);
            OpenLogCommand = new RelayCommand(OpenLog);
            ForceStopCommand = new RelayCommand(ForceStop);

            IsDeadLockable = true;
            IsLiveLockable = true;

            _cancellationSource = new CancellationTokenSource();
        }

        private void CreateNewTransfer(object _)
        {
            TransferWindow transferWindow = new TransferWindow();
            transferWindow.DataContext =
                new TransferWindowVM(TransferUnitUCList, TransferManagerList, TransferUnitList, transferWindow);

            transferWindow.Show();
        }

        private async Task StartTransfer(object _)
        {
            CancellationToken cancellationToken = _cancellationSource.Token;

            bool didComplete = await ParallelTransferUtil
                .StartParallelTransfer(TransferUnitList, IsDeadLockable, IsLiveLockable, cancellationToken);

            if(didComplete)
            {
                MessageBox.Show(
                    "Всички преводи се изпълниха успешно!",
                    "Усхешни преводи",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                return;
            }

            MessageBox.Show(
                    "Всички преводи бяха принудително спряни!",
                    "Неусхешни преводи",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop
                );
        }

        private void CreateAccount(object _)
        {
            CreateAccountWindow createAccountWindow = new CreateAccountWindow();
            createAccountWindow.DataContext =
                new CreateAccountVM(TransferManagerList, createAccountWindow);
            createAccountWindow.Show();
        }

        private void OpenLog(object _)
        {
            LogWindow logWindow = new LogWindow();
            logWindow.Show();
        }

        private void ForceStop(object _)
        {
            _cancellationSource.Cancel();
            _cancellationSource = new CancellationTokenSource();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
