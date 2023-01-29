﻿using DeadlockLivelock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.Utils
{
    public class ParallelTransferUtil
    {
        public static async void StartParallelTransfer(IEnumerable<TransferUnit> transferUnits)
        {
           Task[] tasks = transferUnits
                .Select(transferUnit => Task.Run(async() =>
                {
                    while (true)
                    {
                        if (await TransferManager.Transfer(
                            transferUnit.From,
                            transferUnit.To,
                            transferUnit.Amount
                        )) break;
                    }
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }
    }
}
