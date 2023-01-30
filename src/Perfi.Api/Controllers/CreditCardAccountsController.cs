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
    public class CreditCardAccountsController : ControllerBase
    {
        private readonly IAddCreditCardAccountService _addCreditCardAccountService;
        private readonly ICreditCardAccountQueryService _creditCardAccountQueryService;

        public CreditCardAccountsController(
            IAddCreditCardAccountService addCreditCardAccountService,
            ICreditCardAccountQueryService creditCardAccountQueryService)
        {
            _addCreditCardAccountService = addCreditCardAccountService;
            _creditCardAccountQueryService = creditCardAccountQueryService;
        }



        [HttpPost]
        public async Task<ActionResult<NewCreditCardAccountAddedResponse>> AddAsync([FromBody] AddNewCreditCardAccountCommand addNewCreditCardAccountCommand)
        {
            NewCreditCardAccountAddedResponse newCreditCardAccountAddedResponse = await _addCreditCardAccountService.AddAsync(addNewCreditCardAccountCommand);
            return Created("", newCreditCardAccountAddedResponse);
        }

        [HttpGet]
        public async Task<ActionResult<List<ListCreditCardAccountResponse>>> AllAsync(bool withCurrentBalance)
        {
            List<ListCreditCardAccountResponse> listCreditCardAccountResponses = await _creditCardAccountQueryService.GetAllAsync(withCurrentBalance);
            return Ok(listCreditCardAccountResponses);
        }

        [HttpGet("{creditCardId}/Transactions/CurrentPeriod")]
        public async Task<ActionResult<List<TransactionResponse>>> GetTransactionsAsync(int creditCardId)
        {
            List<TransactionResponse> transactions = await _creditCardAccountQueryService.GetAllTransactionsAsync(creditCardId, TransactionPeriod.CurrentPeriod());
            return Ok(transactions);
        }
    }
}
