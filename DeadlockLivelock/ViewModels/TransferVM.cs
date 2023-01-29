using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadlockLivelock.Models;
using DeadlockLivelock.Utils;

namespace DeadlockLivelock.ViewModels
{
    public class TransferVM : INotifyPropertyChanged
    {
        private TransferManager _transferOneSender;
        private TransferManager _transferOneReciever;
        private TransferManager _transferTwoSender;
        private TransferManager _transferTwoReciever;

        private string _tranferOneAmount;
        private string _tranferTwoAmount;
        private bool _isNotTransfering;

        public string TransferOneAmount
        {
            get { return _tranferOneAmount; }
            set
            {
                _tranferOneAmount = value;
                OnPropertyChanged("TransferOneAmount");
            }
        }

        public string TransferTwoAmount
        {
            get { return _tranferTwoAmount; }
            set
            {
                _tranferTwoAmount = value;
                OnPropertyChanged("TransferTwoAmount");
            }
        }

        public ObservableCollection<TransferManager> Accounts { get; set; }
        public TransferManager TransferOneSender
        {
            get { return _transferOneSender; }
            set
            {
                _transferOneSender = value;
                OnPropertyChanged("TransferOneSender");
            }
        }

        public TransferManager TransferOneReciever
        {
            get { return _transferOneReciever; }
            set
            {
                _transferOneReciever = value;
                OnPropertyChanged("TransferOneReciever");
            }

        }
        public TransferManager TransferTwoSender
        {
            get { return _transferTwoSender; }

            set
            {
                _transferTwoSender = value;
                OnPropertyChanged("TransferTwoSender");
            }

        }
        public TransferManager TransferTwoReciever
        {
            get { return _transferTwoReciever; }
            set
            {
                _transferTwoReciever = value;
                OnPropertyChanged("TransferTwoReciever");
            }

        }

        public RelayCommandAsync StartTransferCommand { get; private set; }
        public bool IsNotTransfering
        {
            get { return _isNotTransfering; }
            set
            {
                _isNotTransfering = value;
                OnPropertyChanged("IsNotTransfering");
            }
        }

        public TransferVM()
        {
            Accounts = new ObservableCollection<TransferManager>();
            TransferManager studentAccount = new TransferManager(new BankAccount(1, 50000, "Student account"));
            TransferManager universityAccount = new TransferManager(new BankAccount(2, 100000, "University account"));

            Accounts.Add(studentAccount);
            Accounts.Add(universityAccount);

            StartTransferCommand = new RelayCommandAsync(StartTransferAsync);
            IsNotTransfering = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async Task StartTransferAsync(object _)
        {
            IsNotTransfering = false;

            double tranferOneAmountParsed;
            double tranferTwoAmountParsed;

            double.TryParse(TransferOneAmount, out tranferOneAmountParsed);
            double.TryParse(TransferTwoAmount, out tranferTwoAmountParsed);

            Task t1 = Task.Run(async () =>
            {
                while (true)
                {
                    if (await TransferManager.Transfer(
                        TransferOneSender,
                        TransferOneReciever,
                        tranferOneAmountParsed
                    )) break;
                }
            });

            Task t2 = Task.Run(async () =>
            {
                while (true)
                {
                    if (await TransferManager.Transfer(
                        TransferTwoSender,
                        TransferTwoReciever,
                        tranferTwoAmountParsed
                    )) break;
                }
            });


            await Task.WhenAll(new[] { t1, t2 });
            IsNotTransfering = true;
        }
    }
}
