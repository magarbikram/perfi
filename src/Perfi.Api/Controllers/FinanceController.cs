using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Services;
using Perfi.Core.Expenses;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly ICloseCurrentTransactionPeriodService _closeCurrentTransactionPeriodService;

        public FinanceController(ICloseCurrentTransactionPeriodService closeCurrentTransactionPeriodService)
        {
            _closeCurrentTransactionPeriodService = closeCurrentTransactionPeriodService;
        }
        [HttpPost("CloseTransactionPeriod/{year}/{month}")]
        public async Task<ActionResult> CloseTransactionPeriod(int year, int month)
        {
            DateTimeOffset transactionPeriodDate = new(year, month, 1, 1, 1, 1, TimeSpan.Zero);
            TransactionPeriod transactionPeriod = TransactionPeriod.For(transactionPeriodDate);
            await _closeCurrentTransactionPeriodService.CloseAsync(transactionPeriod);
            return Ok();
        }
    }
}
