using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
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
    }
}
