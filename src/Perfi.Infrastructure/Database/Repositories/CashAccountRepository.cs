using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class CashAccountRepository : ICashAccountRepository
    {
        private readonly AppDbContext _appDbContext;

        public CashAccountRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IUnitOfWork UnitOfWork => _appDbContext;

        public CashAccount Add(CashAccount cashAccount)
        {
            return _appDbContext.Add(cashAccount).Entity;
        }

        public async Task<IEnumerable<AccountNumber>> GetAllAccountNumbersAsync()
        {
            return await _appDbContext.CashAccounts.Select(ca => ca.AssociatedAccountNumber).ToListAsync();
        }

        public async Task<List<CashAccount>> GetAllAsync()
        {
            return await _appDbContext.CashAccounts.ToListAsync();
        }

        public async Task<Maybe<CashAccount>> GetByIdAsync(int cashAccountId)
        {
            CashAccount? creditCardAccount = await _appDbContext.CashAccounts.FirstOrDefaultAsync(cca => cca.Id == cashAccountId);
            if (creditCardAccount == null)
            {
                return Maybe<CashAccount>.None;
            }
            return creditCardAccount;
        }
    }
}
