using DeadlockLivelock.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.DAL
{
    public class BankAccountContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public BankAccountContext() : base(Properties.Settings.Default.DbConnect) { }
    }
}
