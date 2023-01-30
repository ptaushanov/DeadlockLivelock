using DeadlockLivelock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeadlockLivelock.DAL
{
    public class BankAccountService
    {
        private static readonly BankAccountContext _bankAccountContext 
            = new BankAccountContext();

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public static async Task<int> SaveAccountAsync(BankAccount bankAcount)
        {
            _bankAccountContext.BankAccounts.Add(bankAcount);
            return await _bankAccountContext.SaveChangesAsync();
        }

        public static async Task<int> SaveChangesAsync(int delayAmount)
        {
            await _semaphore.WaitAsync();
            try
            {
                await Task.Delay(delayAmount);
                return await _bankAccountContext.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public static IEnumerable<BankAccount> LoadAccounts()
        {
            return _bankAccountContext.BankAccounts.AsEnumerable();
        }

        public static async Task<int> DeleteAccountAsync(BankAccount bankAccount)
        {
            _bankAccountContext.BankAccounts.Remove(bankAccount);
            return await _bankAccountContext.SaveChangesAsync();
        }
    }
}
