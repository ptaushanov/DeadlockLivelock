﻿using DeadlockLivelock.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.ViewModels
{
    public class ViewAccountsVM : INotifyPropertyChanged
    {
        private TransferManager _selectedTransferManager;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TransferManager> TransferManagerList { get; private set; }
        public RelayCommand DeleteAccountCommand { get; private set; }

        public TransferManager SelectedTransferManager
        {
            get { return _selectedTransferManager; }
            set
            {
                _selectedTransferManager = value;
                OnPropertyChanged("SelectedTransferManager");
                OnPropertyChanged("IsAccountSelected");
            }
        }

        public bool IsAccountSelected
        {
            get { return _selectedTransferManager != null; }
        }

        public ViewAccountsVM(ObservableCollection<TransferManager> transferManagerList)
        {
            TransferManagerList = transferManagerList;
            DeleteAccountCommand = new RelayCommand(DeleteAccount);
        }

        private void DeleteAccount(object _)
        {
            TransferManagerList.Remove(SelectedTransferManager);
            SelectedTransferManager = null;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
