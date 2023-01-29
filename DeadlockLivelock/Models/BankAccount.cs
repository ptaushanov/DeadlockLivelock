using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.Models
{
    public class BankAccount
    {
        public int? BankAccountId { get; set; }
        public string AccountName { get; set; }
        public double Balance { get; set; }

        public BankAccount(int? bankAccountId, double balance, string accountName)
        {
            BankAccountId = bankAccountId;
            Balance = balance;
            AccountName = accountName;
        }
    }
}
