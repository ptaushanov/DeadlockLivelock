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
        public RelayCommand AddCUCommand { get; private set; }
        public ObservableCollection<ComputeUnitUC> ComputeUnitList { get; private set;} 

        public bool PendingOperations { get; set; }
        
        public MainVM()
        {
            ComputeUnitList = new ObservableCollection<ComputeUnitUC>();
            AddCUCommand = new RelayCommand(AddComputeUnit);
            PendingOperations = false;
        }

        private void AddComputeUnit (object _)
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
