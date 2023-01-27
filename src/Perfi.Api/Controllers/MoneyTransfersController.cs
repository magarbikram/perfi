using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;
using Perfi.Core.Expenses;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoneyTransfersController : ControllerBase
    {
        private readonly ITransferMoneyService _transferMoneyService;
        private readonly IMoneyTransferQueryService _moneyTransferQueryService;
        private readonly IPayCreditCardService _payCreditCardService;

        public MoneyTransfersController(
            ITransferMoneyService transferMoneyService,
            IMoneyTransferQueryService moneyTransferQueryService,
            IPayCreditCardService payCreditCardService)
        {
            _transferMoneyService = transferMoneyService;
            _moneyTransferQueryService = moneyTransferQueryService;
            _payCreditCardService = payCreditCardService;
        }

        [HttpPost]
        public async Task<ActionResult<NewMoneyTransferResponse>> TransferAsync([FromBody] TransferMoneyCommand transferMoneyCommand)
        {
            return Created("", await _transferMoneyService.TransferAsync(transferMoneyCommand));
        }


        [HttpPost("PayCreditCard")]
        public async Task<ActionResult<NewMoneyTransferResponse>> PayCreditCardAsync([FromBody] PayCreditCardCommand payCreditCardCommand)
        {
            NewMoneyTransferResponse newMoneyTransferResponse = await _payCreditCardService.PayAsync(payCreditCardCommand);
            return Created("", newMoneyTransferResponse);
        }

        [HttpGet("CurrentPeriod")]
        public async Task<ActionResult<IEnumerable<ListMoneyTransferResponse>>> ListCurrentTransfersAsync()
        {
            IEnumerable<ListMoneyTransferResponse> listExpenseResponses = await _moneyTransferQueryService.GetAllTransfersAsync(TransactionPeriod.CurrentPeriod());
            return Ok(listExpenseResponses);
        }

        [HttpGet("CurrentPeriod/{recordCount}")]
        public async Task<ActionResult<IEnumerable<ListMoneyTransferResponse>>> ListCurrentTopExpensesAsync([FromRoute] int recordCount)
        {
            if (recordCount <= 0)
            {
                ModelState.AddModelError("recordCount", "recordCount should be greater than zero");
                return BadRequest(ModelState);
            }
            IEnumerable<ListMoneyTransferResponse> listExpenseResponses = await _moneyTransferQueryService.GetLimitedTransfersAsync(TransactionPeriod.CurrentPeriod(), recordCount);
            return Ok(listExpenseResponses);
        }
    }
}
