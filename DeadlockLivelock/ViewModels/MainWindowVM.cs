using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadlockLivelock.Models;
using DeadlockLivelock.Utils;
using DeadlockLivelock.Views;

namespace DeadlockLivelock.ViewModels
{
    public class MainWindowVM
    {
        public RelayCommand CreateNewTransferCommand { get; private set; }
        public RelayCommand StartTransferCommand { get; private set; }
        public List<TransferUnit> TransferUnitList { get; private set; }
        public ObservableCollection<TransferUnitUC> TransferUnitUCList { get; private set; }
        public ObservableCollection<TransferManager> TransferManagerList { get; private set; }

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
            StartTransferCommand = new RelayCommand(StartTransfer);
        }

        private void CreateNewTransfer(object _)
        {
            TransferWindow transferWindow = new TransferWindow();
            transferWindow.DataContext =
                new TransferWindowVM(TransferUnitUCList, TransferManagerList, TransferUnitList, transferWindow);

            transferWindow.Show();
        }

        private void StartTransfer(object _)
        {
            ParallelTransferUtil.StartParallelTransfer(TransferUnitList);
        }

    }
}
