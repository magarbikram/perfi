using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class TransactionalAccountRepository : ITransactionalAccountRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public TransactionalAccountRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public TransactionalAccount Add(TransactionalAccount transactionalAccount)
        {
            return _appDbContext.TransactionalAccounts.Add(transactionalAccount).Entity;
        }
    }
}
