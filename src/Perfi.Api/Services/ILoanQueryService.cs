using Perfi.Api.Responses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public interface ILoanQueryService
    {
        Task<List<ListLoanResponse>> GetAllAsync(bool withCurrentBalance);
        Task<List<TransactionResponse>> GetAllTransactionsAsync(int creditCardId, TransactionPeriod transactionPeriod);
    }
}