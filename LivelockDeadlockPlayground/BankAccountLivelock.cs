using System;
using System.Threading;
using System.Threading.Tasks;

namespace LivelockDeadlockPlayground
{
    public class BankAccountLivelock
    {
        private double _balance;
        private int _id;
        private object _flag;

        public int Id { get { return _id; } }

        public BankAccountLivelock(int id, double balance)
        {
            _id = id;
            _balance = balance;
            _flag = new object();
        }

        public async Task<bool> WithdrawAsync(double amount)
        {
            if (Monitor.TryEnter(_flag))
            {
                await Task.Delay(1000);
                _balance -= amount;
                return true;
            }
            return false;
        }

        public async Task<bool> DepositAsync(double amount)
        {
            if (Monitor.TryEnter(_flag))
            {
                await Task.Delay(1000);
                _balance += amount;
                return true;
            }
            return false;

        }

        public static async Task<bool> Transfer(BankAccountLivelock from, BankAccountLivelock to, double amount)
        {
            //bool didWithdraw = await from.WithdrawAsync(amount);

            // if(!didWithdraw) return false;
            // Console.WriteLine("Withdrawing " + amount + " from " + from.Id);

            // bool didDeposit = await to.DepositAsync(amount);

            // if (!didDeposit) {
            //     Console.WriteLine("Refunding " + amount + " to " + from.Id);
            //     await from.DepositAsync(amount);
            //     return false;
            // };


            // Console.WriteLine("Depositing " + amount + " to " + to.Id);
            // return true;


            #region OldCode
            if (await from.WithdrawAsync(amount))
            {
                Console.WriteLine("Withdrawing " + amount + " from " + from.Id);
                if (await to.DepositAsync(amount))
                {
                    Console.WriteLine("Depositing " + amount + " to " + to.Id);
                    return true;
                }
                else
                {
                    await from.DepositAsync(amount);
                    Console.WriteLine("Refunding " + amount + " to " + from.Id);
                }
            }
            #endregion
            return false;
        }
    }
}
