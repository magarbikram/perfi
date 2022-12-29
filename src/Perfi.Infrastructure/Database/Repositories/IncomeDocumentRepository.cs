using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounting;
using Perfi.Core.Earnings;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class IncomeDocumentRepository : IIncomeDocumentRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public IncomeDocumentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IncomeDocument Add(IncomeDocument incomeDocument)
        {
            return _appDbContext.IncomeDocuments.Add(incomeDocument).Entity;
        }

        public void Update(IncomeDocument incomeDocument)
        {
            _appDbContext.IncomeDocuments.Update(incomeDocument);
        }

        public async Task<IEnumerable<IncomeDocument>> GetAllForTransactionPeriodAsync(TransactionPeriod transactionPeriod)
        {
            return await _appDbContext.IncomeDocuments.Include(inc => inc.PaymentDeposition).Where(inc => inc.TransactionPeriod == transactionPeriod).ToListAsync();
        }

        public async Task<IEnumerable<IncomeDocument>> GetTop10IncomesForTransactionPeriodAsync(TransactionPeriod transactionPeriod)
        {
            return await _appDbContext.IncomeDocuments
                                      .Include(inc => inc.PaymentDeposition)
                                      .Where(inc => inc.TransactionPeriod == transactionPeriod)
                                      .OrderByDescending(exp => exp.TransactionDate)
                                      .Take(10)
                                      .ToListAsync();
        }

        public async Task<Money> GetTotalIncomeAmountForPeriodAsync(TransactionPeriod currentTransactionPeriod)
        {
            decimal totalIncomeAmountValue = await _appDbContext.IncomeDocuments
                                                        .Where(inc => inc.TransactionPeriod == currentTransactionPeriod)
                                                        .Select(inc => inc.Amount)
                                                        .SumAsync(ina => ina.Value);
            return Money.UsdFrom(totalIncomeAmountValue);
        }
    }
}
