using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;
using System.Reflection;

namespace Perfi.Infrastructure.Database
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        public DbSet<CashAccount> CashAccounts { get; set; }
        public DbSet<CreditCardAccount> CreditCardAccounts { get; set; }
        public DbSet<SummaryAccount> SummaryAccounts { get; set; }
        public DbSet<TransactionalAccount> TransactionalAccounts { get; set; }
        public DbSet<SummaryExpenseCategory> SummaryExpenseCategories { get; set; }
        public DbSet<TransactionalExpenseCategory> TransactionalExpenseCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Loan> Loans { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
