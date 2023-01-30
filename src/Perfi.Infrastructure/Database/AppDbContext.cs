using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Core.Earnings;
using Perfi.Core.Earnings.IncomeSources;
using Perfi.Core.Expenses;
using Perfi.Core.MoneyTransfers;
using Perfi.Core.Payments.IncomingPayments;
using Perfi.Core.Payments.LoanPayments;
using Perfi.Core.Payments.OutgoingPayments;
using Perfi.Core.SplitPartners;
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
        public DbSet<AccountingTransaction> AccountingTransactions { get; set; }
        public DbSet<AccountingEntry> AccountingEntries { get; set; }
        public DbSet<SummaryExpenseCategory> SummaryExpenseCategories { get; set; }
        public DbSet<TransactionalExpenseCategory> TransactionalExpenseCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<IncomeSource> IncomeSources { get; set; }
        public DbSet<IncomeDocument> IncomeDocuments { get; set; }
        public DbSet<SplitPartner> SplitPartners { get; set; }
        public DbSet<MoneyTransfer> MoneyTransfers { get; set; }
        public DbSet<IncomingPayment> IncomingPayments { get; set; }
        public DbSet<OutgoingPayment> OutgoingPayments { get; set; }
        public DbSet<LoanPayment> LoanPayments { get; set; }
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
