using Perfi.Api.Responses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public interface IMoneyTransferQueryService
    {
        Task<IEnumerable<ListMoneyTransferResponse>> GetAllTransfersAsync(TransactionPeriod transactionPeriod);
        Task<IEnumerable<ListMoneyTransferResponse>> GetLimitedTransfersAsync(TransactionPeriod transactionPeriod, int count);
    }
}