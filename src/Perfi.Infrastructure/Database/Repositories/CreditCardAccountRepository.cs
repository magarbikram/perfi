using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class CreditCardAccountRepository : ICreditCardAccountRepository
    {
        private readonly AppDbContext _appDbContext;

        public CreditCardAccountRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IUnitOfWork UnitOfWork => _appDbContext;

        public CreditCardAccount Add(CreditCardAccount creditCardAccount)
        {
            return _appDbContext.Add(creditCardAccount).Entity;
        }

        public async Task<List<CreditCardAccount>> GetAllAsync()
        {
            return await _appDbContext.CreditCardAccounts.ToListAsync();
        }
    }
}
