using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;
using Perfi.Core.Expenses;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IAddLoanService _addLoanService;
        private readonly ILoanQueryService _loanQueryService;
        private readonly IPayLoanService _payLoanService;

        public LoansController(
            IAddLoanService addLoanService,
            ILoanQueryService loanQueryService,
            IPayLoanService payLoanService)
        {
            _addLoanService = addLoanService;
            _loanQueryService = loanQueryService;
            _payLoanService = payLoanService;
        }
        [HttpPost]
        public async Task<ActionResult<NewLoanAddedResponse>> AddAsync([FromBody] AddNewLoanCommand addNewLoanCommand)
        {
            NewLoanAddedResponse newLoanAddedResponse = await _addLoanService.AddAsync(addNewLoanCommand);
            return Created("", newLoanAddedResponse);
        }

        [HttpGet]
        public async Task<ActionResult<List<ListLoanResponse>>> AllAsync(bool withCurrentBalance)
        {
            List<ListLoanResponse> listLoanResponses = await _loanQueryService.GetAllAsync(withCurrentBalance);
            return Ok(listLoanResponses);
        }

        [HttpPost("Pay")]
        public async Task<ActionResult<NewLoanPaymentAddedResponse>> PayLoanAsync([FromBody] PayLoanCommand payMortgageCommand)
        {
            NewLoanPaymentAddedResponse newLoanPaymentAddedResponse = await _payLoanService.PayAsync(payMortgageCommand);
            return Created("", newLoanPaymentAddedResponse);
        }


        [HttpGet("{loanId}/Transactions/CurrentPeriod")]
        public async Task<ActionResult<List<TransactionResponse>>> GetTransactionsAsync(int loanId)
        {
            List<TransactionResponse> transactions = await _loanQueryService.GetAllTransactionsAsync(loanId, TransactionPeriod.CurrentPeriod());
            return Ok(transactions);
        }
    }
}
