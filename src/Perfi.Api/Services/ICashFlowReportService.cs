using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface ICashFlowReportService
    {
        Task<CashFlowSummaryResponse> GetCurrentPeriodCashFlowSummaryAsync();
    }
}