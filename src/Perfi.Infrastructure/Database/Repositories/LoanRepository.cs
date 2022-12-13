using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _appDbContext;

        public LoanRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IUnitOfWork UnitOfWork => _appDbContext;

        public Loan Add(Loan loan)
        {
            return _appDbContext.Add(loan).Entity;
        }

        public async Task<List<Loan>> GetAllAsync()
        {
            return await _appDbContext.Loans.ToListAsync();
        }
    }
}
