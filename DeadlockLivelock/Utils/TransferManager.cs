using DeadlockLivelock.DAL;
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
        private readonly object _flag;
        private readonly SemaphoreSlim _semaphore;

        public event PropertyChangedEventHandler PropertyChanged;

        public BankAccount ManagedAccount { get; private set; }

        public TransferManager(BankAccount account)
        {
            _flag = new object();
            _semaphore = new SemaphoreSlim(1, 1);
            ManagedAccount = account;
        }

        public async Task<bool> WithdrawCorrectedAsync(double amount)
        {
            await _semaphore.WaitAsync();
            try
            {
                ManagedAccount.Balance -= amount;
                //await Task.Delay(1000);
                await BankAccountService.SaveChangesAsync(1000);
                OnPropertyChanged("ManagedAccount");
                return true;
            }
            catch { return false; }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> WithdrawAsync(
            double amount,
            bool isDeadLockable,
            bool isLiveLockable
        )
        {
            if (!isDeadLockable && !isLiveLockable)
                return await WithdrawCorrectedAsync(amount);

            // Deadlocable senario
            // Livelocable senario
            if (Monitor.TryEnter(_flag))
            {
                ManagedAccount.Balance -= amount;

                if (isDeadLockable && !isLiveLockable)
                    //await Task.Delay(1000);
                    await BankAccountService.SaveChangesAsync(1000);
                else
                    //Task.Delay(1000).Wait();
                    BankAccountService.SaveChangesAsync(1000).Wait();

                OnPropertyChanged("ManagedAccount");
                return true;
            }
            return false;
        }

        public async Task<bool> DepositCorrectedAsync(double amount)
        {
            await _semaphore.WaitAsync();
            try
            {
                ManagedAccount.Balance += amount;
                //await Task.Delay(1000);
                await BankAccountService.SaveChangesAsync(1000);
                OnPropertyChanged("ManagedAccount");
                return true;
            }
            catch { return false; }
            finally
            {
                _semaphore.Release();
            }
        }


        public async Task<bool> DepositAsync(
            double amount,
            bool isDeadLockable,
            bool isLiveLockable
        )
        {
            if (!isDeadLockable && !isLiveLockable)
                return await DepositCorrectedAsync(amount);

            // Deadlocable senario
            // Livelocable senario
            if (Monitor.TryEnter(_flag))
            {
                ManagedAccount.Balance += amount;

                if (isDeadLockable && !isLiveLockable)
                    //await Task.Delay(1000);
                    await BankAccountService.SaveChangesAsync(1000);

                else
                    //Task.Delay(1000).Wait();
                    BankAccountService.SaveChangesAsync(1000).Wait();

                OnPropertyChanged("ManagedAccount");
                return true;
            }
            return false;

        }

        public static async Task<bool> Transfer(
            TransferManager from,
            TransferManager to,
            double amount,
            bool isDeadLockable,
            bool isLiveLockable
        )
        {
            bool didWithdraw =
                await from.WithdrawAsync(amount, isDeadLockable, isLiveLockable);

            if (!didWithdraw) return false;
            Logger.Log("Изтегляне на " + amount + "лв от сметка " + from.ManagedAccount.AccountName);
            Debug.WriteLine("Withdrawing " + amount + " from " + from.ManagedAccount.BankAccountId);

            bool didDeposit =
                await to.DepositAsync(amount, isDeadLockable, isLiveLockable);

            if (!didDeposit)
            {
                Logger.Log("Връщане на " + amount + "лв обратно в сметка " + from.ManagedAccount.AccountName);
                Debug.WriteLine("Refunding " + amount + " to " + from.ManagedAccount.BankAccountId);

                await from
                    .DepositAsync(amount, isDeadLockable, isLiveLockable);
                return false;
            };

            Logger.Log("Добавяне на " + amount + "лв в сметка " + to.ManagedAccount.AccountName);
            Debug.WriteLine("Depositing " + amount + " to " + to.ManagedAccount.BankAccountId);
            return true;
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
