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
    public class MainVM
    {
        public RelayCommand CreateNewTransferCommand { get; private set; }
        public ObservableCollection<ComputeUnitUC> ComputeUnitList { get; private set;} 

        public bool PendingOperations { get; set; }
        
        public MainVM()
        {
            ComputeUnitList = new ObservableCollection<ComputeUnitUC>();
            CreateNewTransferCommand = new RelayCommand(CreateNewTransfer);
            PendingOperations = false;
        }

        private void CreateNewTransfer (object _)
        {
            ComputeUnit newCU = new ComputeUnit(null, "New CU", "New");
            // TODO: save to db maybe

            var newCUUC = new ComputeUnitUC
            {
                DataContext = new ComputeUnitUCVM(newCU, this)
            };
            ComputeUnitList.Add(newCUUC);
        }

    }
}
