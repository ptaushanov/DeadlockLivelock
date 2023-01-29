using DeadlockLivelock.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeadlockLivelock.Utils
{
    public class TransferManager : INotifyPropertyChanged
    {
        private object _flag;

        public event PropertyChangedEventHandler PropertyChanged;

        public BankAccount ManagedAccount { get; private set; }


        public TransferManager(BankAccount account)
        {
            _flag = new object();
            ManagedAccount = account;
        }

        public async Task<bool> WithdrawAsync(double amount)
        {
            if (Monitor.TryEnter(_flag))
            {
                Task.Delay(1000).Wait();
                ManagedAccount.Balance -= amount;
                OnPropertyChanged("ManagedAccount");
                return true;
            }
            return false;
        }

        public async Task<bool> DepositAsync(double amount)
        {
            if (Monitor.TryEnter(_flag))
            {
                Task.Delay(1000).Wait();
                ManagedAccount.Balance += amount;
                OnPropertyChanged("ManagedAccount");
                return true;
            }
            return false;

        }

        public static async Task<bool> Transfer(TransferManager from, TransferManager to, double amount)
        {
            bool didWithdraw = await from.WithdrawAsync(amount);

            if (!didWithdraw) return false;
            Debug.WriteLine("Withdrawing " + amount + " from " + from.ManagedAccount.BankAccountId);

            bool didDeposit = await to.DepositAsync(amount);

            if (!didDeposit)
            {
                Debug.WriteLine("Refunding " + amount + " to " + from.ManagedAccount.BankAccountId);
                await from.DepositAsync(amount);
                return false;
            };


            Debug.WriteLine("Depositing " + amount + " to " + to.ManagedAccount.BankAccountId);
            return true;
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
