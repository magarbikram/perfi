using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface ISummaryExpenseCategoryQueryService
    {
        Task<IEnumerable<ListSummaryExpenseCategoryResponse>> GetAllAsync();
        Task<ListExpenseAccountResponse> GetAssociatedExpenseAccountAsync(string summaryExpenseCategoryCode);
    }
}