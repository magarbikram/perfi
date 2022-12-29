using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private readonly IAddNewIncomeService _addIncomeService;
        private readonly IIncomeDocumentQueryService _incomeDocumentQueryService;

        public IncomesController(
            IAddNewIncomeService addIncomeService,
            IIncomeDocumentQueryService incomeDocumentQueryService)
        {
            _addIncomeService = addIncomeService;
            _incomeDocumentQueryService = incomeDocumentQueryService;
        }
        [HttpPost]
        public async Task<ActionResult<NewIncomeAddedResponse>> AddAsync([FromBody] AddNewIncomeCommand addNewIncomeCommand)
        {
            return Created("", await _addIncomeService.AddAsync(addNewIncomeCommand));
        }

        [HttpGet("CurrentPeriod")]
        public async Task<ActionResult<IEnumerable<ListIncomeResponse>>> ListCurrentIncomesAsync()
        {
            IEnumerable<ListIncomeResponse> listIncomeResponses = await _incomeDocumentQueryService.GetCurrentIncomesAsync();
            return Ok(listIncomeResponses);
        }

        [HttpGet("CurrentPeriod/TopTen")]
        public async Task<ActionResult<IEnumerable<ListIncomeResponse>>> ListCurrentTop10IncomesAsync()
        {
            IEnumerable<ListIncomeResponse> listIncomeResponses = await _incomeDocumentQueryService.GetCurrentTop10IncomesAsync();
            return Ok(listIncomeResponses);
        }
    }
}
