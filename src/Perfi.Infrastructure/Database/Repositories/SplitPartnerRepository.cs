using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.SplitPartners;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class SplitPartnerRepository : ISplitPartnerRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public SplitPartnerRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public SplitPartner Add(SplitPartner splitPartner)
        {
            return _appDbContext.SplitPartners.Add(splitPartner).Entity;
        }

        public async Task<IEnumerable<SplitPartner>> GetAllAsync()
        {
            return await _appDbContext.SplitPartners.ToListAsync();
        }

        public async Task<Maybe<SplitPartner>> GetByIdAsync(int id)
        {
            SplitPartner? splitPartner = await _appDbContext.SplitPartners.FirstOrDefaultAsync(sp => sp.Id == id);
            return splitPartner.AsMaybe();
        }

        public async Task<IEnumerable<AccountNumber>> GetAllAccountNumbersAsync()
        {
            return await _appDbContext.SplitPartners.Select(sp => sp.ReceivableAccountNumber).ToListAsync();
        }
    }
}
