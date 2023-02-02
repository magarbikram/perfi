using Perfi.Api.Responses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public interface IExpenseQueryService
    {
        Task<IEnumerable<ListExpenseResponse>> GetCurrentExpensesAsync();
        Task<IEnumerable<ExpenseBySummaryCategoryResponse>> GetCurrentExpensesByCategoryAsync();
        Task<IEnumerable<ListExpenseResponse>> GetCurrentTop10ExpensesAsync();
        Task<IEnumerable<ExpenseBySummaryCategoryResponse>> GetExpensesByCategoryAsync(TransactionPeriod transactionPeriod);
    }
}