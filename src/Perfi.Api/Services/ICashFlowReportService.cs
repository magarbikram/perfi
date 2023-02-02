using Perfi.Api.Responses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public interface ICashFlowReportService
    {
        Task<CashFlowSummaryResponse> GetCashFlowSummaryAsync(TransactionPeriod transactionPeriod);
        Task<CashFlowSummaryResponse> GetCurrentPeriodCashFlowSummaryAsync();
    }
}