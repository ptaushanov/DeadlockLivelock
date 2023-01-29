using DeadlockLivelock.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.Models
{
    public class TransferUnit : INotifyPropertyChanged
    {
        public static int TransferNumberCount { get; private set; } = 0;

        private TransferManager _from;
        private TransferManager _to;
        private double _amount;
        private TransferStatus _status;

        public TransferManager From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged("From");
            }
        }

        public TransferManager To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged("To");
            }

        }

        public TransferStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }

        }

        public double Amount { 
            get { return _amount; } 
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public int TransferNumber { get; private set; }

        public TransferUnit(TransferManager from, TransferManager to, double amount)
        {
            From = from;
            To = to;
            Amount = amount;
            Status = TransferStatus.PENDING;
            TransferNumber = ++TransferNumberCount;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
