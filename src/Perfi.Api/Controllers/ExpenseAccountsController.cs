using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseAccountsController : ControllerBase
    {
        private readonly IAddExpenseAccountService _addExpenseAccountService;
        private readonly IExpenseAccountQueryService _expenseAccountQueryService;

        public ExpenseAccountsController(
            IAddExpenseAccountService addExpenseAccountService,
            IExpenseAccountQueryService expenseAccountQueryService)
        {
            _addExpenseAccountService = addExpenseAccountService;
            _expenseAccountQueryService = expenseAccountQueryService;
        }
        [HttpPost]
        public async Task<ActionResult<NewExpenseAccountAddedResponse>> AddAsync([FromBody] AddNewExpenseAccountCommand addNewExpenseAccountCommand)
        {
            NewExpenseAccountAddedResponse newExpenseAccountAddedResponse = await _addExpenseAccountService.AddAsync(addNewExpenseAccountCommand);
            return Created("", newExpenseAccountAddedResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListExpenseAccountResponse>>> AllAsync()
        {
            IEnumerable<ListExpenseAccountResponse> listExpenseAccountResponse = await _expenseAccountQueryService.GetAllAsync();
            return Ok(listExpenseAccountResponse);
        }
    }
}
