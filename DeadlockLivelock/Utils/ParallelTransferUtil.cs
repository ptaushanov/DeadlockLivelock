using DeadlockLivelock.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeadlockLivelock.Utils
{
    public class ParallelTransferUtil
    {
        public static async Task<bool> StartParallelTransfer(
            IEnumerable<TransferUnit> transferUnits,
            bool isDeadLockable,
            bool isLiveLockable,
            CancellationToken cancellationToken
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
                         if (cancellationToken.IsCancellationRequested)
                         {
                             transferUnit.Status = TransferStatus.CANCELED;
                             break;
                         }

                         if (await TransferManager.Transfer(
                             transferUnit.From,
                             transferUnit.To,
                             transferUnit.Amount,
                             isDeadLockable,
                             isLiveLockable
                         )) break;
                     }
                     transferUnit.Status = TransferStatus.COMPLEATED;
                 }, cancellationToken))
                 .ToArray();

            try
            {
                await Task.WhenAll(tasks);
                return true;
            }
            catch (TaskCanceledException _)
            {
                return false;
            }
        }
    }
}
