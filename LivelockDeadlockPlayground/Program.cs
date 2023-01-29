using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivelockDeadlockPlayground
{
    internal class Program
    {

        static void Main(string[] args)
        {
            BankAccountLivelock studentBankAccount = new BankAccountLivelock(1, 50000);
            BankAccountLivelock universityBanckAccount = new BankAccountLivelock(2, 100000);

            var t1 = Task.Run(async () =>
            {
                while (true)
                {
                    if (await BankAccountLivelock.Transfer(studentBankAccount, universityBanckAccount, 3000)) break;
                }

                Console.WriteLine("Transfer 1 completed!");
            });


            var t2 = Task.Run(async () =>
            {
                while (true)
                {
                    if (await BankAccountLivelock.Transfer(universityBanckAccount, studentBankAccount, 1000)) break;
                }
                Console.WriteLine("Transfer 2 completed!");

            });


            Task.WaitAll(new[] { t1, t2 });
        }
    }

}
