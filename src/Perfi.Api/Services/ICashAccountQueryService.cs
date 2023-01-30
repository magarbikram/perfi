using Perfi.Api.Responses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public interface ICashAccountQueryService
    {
        Task<List<ListCashAccountResponse>> GetAllAsync(bool withCurrentBalance);
        Task<List<TransactionResponse>> GetAllTransactionsAsync(int cashAccountId, TransactionPeriod transactionPeriod);
    }
}