using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate.QueryModels;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;
using System.Linq;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class AccountingTransactionRepository : IAccountingTransactionRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public AccountingTransactionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public AccountingTransaction Add(AccountingTransaction accountingTransaction)
        {
            return _appDbContext.AccountingTransactions.Add(accountingTransaction).Entity;
        }

        public async Task<IEnumerable<AccountingEntry>> GetAccountingEntriesOfPeriodAsync(AccountNumber accountNumber, TransactionPeriod transactionPeriod)
        {
            return await _appDbContext.AccountingEntries.Where(ae => ae.TransactionPeriod == transactionPeriod && ae.AccountNumber == accountNumber).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAccountingTransactionsOfPeriodAsync(AccountNumber accountNumber, TransactionPeriod transactionPeriod)
        {
            var query = from accountingEntry in _appDbContext.AccountingEntries.Where(ae => ae.AccountNumber == accountNumber && ae.TransactionPeriod == transactionPeriod)
                        join accountingTransaction in _appDbContext.AccountingTransactions
                        on EF.Property<int>(accountingEntry, "AccountingTransactionId") equals accountingTransaction.Id
                        select Transaction.From(accountingTransaction.Id,
                                                accountingTransaction.Description,
                                                accountingTransaction.TransactionDate,
                                                accountingEntry.DebitAmount,
                                                accountingEntry.CreditAmount
                                                );
            return await query.AsNoTracking().ToListAsync();
        }
    }
}
