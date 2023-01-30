using Perfi.Api.Responses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public interface ISplitPartnerQueryService
    {
        Task<IEnumerable<ListSplitPartnerResponse>> GetAllAsync();
        Task<List<TransactionResponse>> GetAllTransactionsAsync(int creditCardId, TransactionPeriod transactionPeriod);
        Task<IEnumerable<ListSplitPartnerWithCurrentBalanceResponse>> GetAllWithCurrentBalanceAsync();
    }
}