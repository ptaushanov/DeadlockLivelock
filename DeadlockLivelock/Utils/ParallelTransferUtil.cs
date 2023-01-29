using DeadlockLivelock.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.Utils
{
    public class ParallelTransferUtil
    {
        public static async void StartParallelTransfer(
            IEnumerable<TransferUnit> transferUnits,
            bool isDeadLockable,
            bool isLiveLockable
        )
        {
            Debug.WriteLine(
                "Starting transfer ... \n" +
                "Deadlock mode is " + 
                (isDeadLockable ? "on" : "off")
            );

            Task[] tasks = transferUnits
                 .Select(transferUnit => Task.Run(async () =>
                 {
                     transferUnit.Status = TransferStatus.TRANSFERING;
                     while (true)
                     {
                         if (await TransferManager.Transfer(
                             transferUnit.From,
                             transferUnit.To,
                             transferUnit.Amount,
                             isDeadLockable,
                             isLiveLockable
                         )) break;
                     }
                     transferUnit.Status = TransferStatus.COMPLEATED;
                 }))
                 .ToArray();

            await Task.WhenAll(tasks);
        }
    }
}
