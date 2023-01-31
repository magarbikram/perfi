using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public interface ICloseCurrentTransactionPeriodService
    {
        Task CloseAsync(TransactionPeriod transactionPeriod);
    }
}