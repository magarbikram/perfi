using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryExpenseCategoriesController : ControllerBase
    {
        private readonly IAddSummaryExpenseCategoryService _addSummaryExpenseCategoryService;
        private readonly ISummaryExpenseCategoryQueryService _summaryExpenseCategoryQueryService;

        public SummaryExpenseCategoriesController(
            IAddSummaryExpenseCategoryService addSummaryExpenseCategoryService,
            ISummaryExpenseCategoryQueryService summaryExpenseCategoryQueryService)
        {
            _addSummaryExpenseCategoryService = addSummaryExpenseCategoryService;
            _summaryExpenseCategoryQueryService = summaryExpenseCategoryQueryService;
        }

        [HttpPost]
        public async Task<ActionResult<NewSummaryExpenseCategoryAddedResponse>> AddAsync([FromBody] AddNewSummaryExpenseCategoryCommand addNewSummaryExpenseCategoryCommand)
        {
            NewSummaryExpenseCategoryAddedResponse newSummaryExpenseCategoryAddedResponse = await _addSummaryExpenseCategoryService.AddAsync(addNewSummaryExpenseCategoryCommand);
            return Created("", newSummaryExpenseCategoryAddedResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListSummaryExpenseCategoryResponse>>> AllAsync()
        {
            IEnumerable<ListSummaryExpenseCategoryResponse> listCashAccountResponses = await _summaryExpenseCategoryQueryService.GetAllAsync();
            return Ok(listCashAccountResponses);
        }

        [HttpGet("{summaryExpenseCategoryCode}/AssociatedExpenseAccount")]
        public async Task<ActionResult<ListExpenseAccountResponse>> GetAssociatedExpenseAccount([FromRoute] string summaryExpenseCategoryCode)
        {
            ListExpenseAccountResponse listCashAccountResponse = await _summaryExpenseCategoryQueryService.GetAssociatedExpenseAccountAsync(summaryExpenseCategoryCode);
            return Ok(listCashAccountResponse);
        }
    }
}
