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

        public ExpensesController(
            IAddNewExpenseService addNewExpenseService,
            IExpenseQueryService expenseQueryService)
        {
            _addNewExpenseService = addNewExpenseService;
            _expenseQueryService = expenseQueryService;
        }

        [HttpPost]
        public async Task<ActionResult<NewExpenseAddedResponse>> AddAsync([FromBody] AddNewExpenseCommand addNewExpenseAccountCommand)
        {
            NewExpenseAddedResponse newExpenseAddedResponse = await _addNewExpenseService.AddAsync(addNewExpenseAccountCommand);
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
