using CSharpFunctionalExtensions;
using Perfi.SharedKernel;

namespace Perfi.Core.Expenses
{
    public interface ISummaryExpenseCategoryRepository : IRepository<SummaryExpenseCategory>
    {
        SummaryExpenseCategory Add(SummaryExpenseCategory category);
        Task<IEnumerable<SummaryExpenseCategory>> GetAllAsync(bool includeCategories = false);
        Task<Maybe<SummaryExpenseCategory>> GetByCodeAsync(string summaryExpenseCategoryCode);
    }
}
