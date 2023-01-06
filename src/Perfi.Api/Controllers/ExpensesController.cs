using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IAddNewExpenseService _addNewExpenseService;
        private readonly IExpenseQueryService _expenseQueryService;
        private readonly IPayMortgageService _payMortgageService;
        private readonly IPayCreditCardService _payCreditCardService;
        private readonly IPayLoanService _payLoanService;

        public ExpensesController(
            IAddNewExpenseService addNewExpenseService,
            IExpenseQueryService expenseQueryService,
            IPayMortgageService payMortgageService,
            IPayCreditCardService payCreditCardService,
            IPayLoanService payLoanService)
        {
            _addNewExpenseService = addNewExpenseService;
            _expenseQueryService = expenseQueryService;
            _payMortgageService = payMortgageService;
            _payCreditCardService = payCreditCardService;
            _payLoanService = payLoanService;
        }

        [HttpPost]
        public async Task<ActionResult<NewExpenseAddedResponse>> AddAsync([FromBody] AddNewExpenseCommand addNewExpenseAccountCommand)
        {
            NewExpenseAddedResponse newExpenseAddedResponse = await _addNewExpenseService.AddAsync(addNewExpenseAccountCommand);
            return Created("", newExpenseAddedResponse);
        }


        [HttpPost("PayMortgage")]
        public async Task<ActionResult<NewExpenseAddedResponse>> PayMortgageAsync([FromBody] PayMortgageCommand payMortgageCommand)
        {
            NewExpenseAddedResponse newExpenseAddedResponse = await _payMortgageService.PayAsync(payMortgageCommand);
            return Created("", newExpenseAddedResponse);
        }

        [HttpPost("PayLoan")]
        public async Task<ActionResult<NewExpenseAddedResponse>> PayLoanAsync([FromBody] PayLoanCommand payMortgageCommand)
        {
            NewExpenseAddedResponse newExpenseAddedResponse = await _payLoanService.PayAsync(payMortgageCommand);
            return Created("", newExpenseAddedResponse);
        }

        [HttpPost("PayCreditCard")]
        public async Task<ActionResult<NewExpenseAddedResponse>> PayCreditCardAsync([FromBody] PayCreditCardCommand payCreditCardCommand)
        {
            NewExpenseAddedResponse newExpenseAddedResponse = await _payCreditCardService.PayAsync(payCreditCardCommand);
            return Created("", newExpenseAddedResponse);
        }

        [HttpGet("CurrentPeriod")]
        public async Task<ActionResult<IEnumerable<ListExpenseResponse>>> ListCurrentExpensesAsync()
        {
            IEnumerable<ListExpenseResponse> listExpenseResponses = await _expenseQueryService.GetCurrentExpensesAsync();
            return Ok(listExpenseResponses);
        }

        [HttpGet("CurrentPeriod/TopTen")]
        public async Task<ActionResult<IEnumerable<ListExpenseResponse>>> ListCurrentTop10ExpensesAsync()
        {
            IEnumerable<ListExpenseResponse> listExpenseResponses = await _expenseQueryService.GetCurrentTop10ExpensesAsync();
            return Ok(listExpenseResponses);
        }
    }
}
