using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Earnings.IncomeSources;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class IncomeSourceRepository : IIncomeSourceRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public IncomeSourceRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IncomeSource Add(IncomeSource incomeSource)
        {
            return _appDbContext.IncomeSources.Add(incomeSource).Entity;
        }

        public async Task<IEnumerable<IncomeSource>> GetAllAsync()
        {
            return await _appDbContext.IncomeSources.ToListAsync();
        }

        public async Task<Maybe<IncomeSource>> GetByIdAsync(int id)
        {
            IncomeSource? incomeSource = await _appDbContext.IncomeSources.FirstOrDefaultAsync(x => x.Id == id);
            return incomeSource.AsMaybe();
        }
    }
}
