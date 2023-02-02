using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Responses;
using Perfi.Api.Services;
using Perfi.Core.Expenses;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashFlowsController : ControllerBase
    {
        private readonly ICashFlowReportService _cashFlowReportService;

        public CashFlowsController(ICashFlowReportService cashFlowReportService)
        {
            _cashFlowReportService = cashFlowReportService;
        }

        [HttpGet("CurrentPeriod")]
        public async Task<ActionResult<CashFlowSummaryResponse>> GetCurrentPeriodCashFlowSummaryAsync()
        {
            return Ok(await _cashFlowReportService.GetCurrentPeriodCashFlowSummaryAsync());
        }
        [HttpGet("TransactionPeriod/{year}/{month}")]
        public async Task<ActionResult<CashFlowSummaryResponse>> GetCurrentPeriodCashFlowSummaryAsync(int year, int month)
        {
            TransactionPeriod transactionPeriod = TransactionPeriod.For(year, month);
            return Ok(await _cashFlowReportService.GetCashFlowSummaryAsync(transactionPeriod));
        }
    }
}
