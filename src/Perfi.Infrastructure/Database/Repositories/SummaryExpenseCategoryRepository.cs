using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class SummaryExpenseCategoryRepository : ISummaryExpenseCategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public SummaryExpenseCategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public SummaryExpenseCategory Add(SummaryExpenseCategory category)
        {
            return _appDbContext.Add(category).Entity;
        }

        public async Task<IEnumerable<SummaryExpenseCategory>> GetAllAsync(bool includeCategories = false)
        {
            if (includeCategories)
            {
                return await _appDbContext.SummaryExpenseCategories.Include(sec => sec.Categories).ToListAsync();
            }
            return await _appDbContext.SummaryExpenseCategories.ToListAsync();
        }

        public async Task<Maybe<SummaryExpenseCategory>> GetByCodeAsync(string summaryExpenseCategoryCode)
        {
            SummaryExpenseCategory? summaryExpenseCategory = await _appDbContext.SummaryExpenseCategories.Include(sec => sec.Categories).FirstOrDefaultAsync(sec => sec.Code == ExpenseCategoryCode.From(summaryExpenseCategoryCode));
            if (summaryExpenseCategory == null)
            {
                return Maybe<SummaryExpenseCategory>.None;
            }
            return summaryExpenseCategory;
        }
    }
}
