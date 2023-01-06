using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

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
            return _appDbContext.Add(accountingTransaction).Entity;
        }

        public async Task<IEnumerable<AccountingEntry>> GetAccountingEntriesOfPeriodAsync(AccountNumber accountNumber, TransactionPeriod transactionPeriod)
        {
            return await _appDbContext.AccountingEntries.Where(ae => ae.TransactionPeriod == transactionPeriod && ae.AccountNumber == accountNumber).ToListAsync();
        }
    }
}
