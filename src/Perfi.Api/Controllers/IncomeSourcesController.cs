using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeSourcesController : ControllerBase
    {
        private readonly IAddNewIncomeSourceService _addNewIncomeSourceService;
        private readonly IIncomeSourceQueryService _incomeSourceQueryService;

        public IncomeSourcesController(
            IAddNewIncomeSourceService addNewIncomeSourceService,
            IIncomeSourceQueryService incomeSourceQueryService)
        {
            _addNewIncomeSourceService = addNewIncomeSourceService;
            _incomeSourceQueryService = incomeSourceQueryService;
        }

        [HttpPost]
        public async Task<ActionResult<NewIncomeSourceAddedResponse>> AddAsync([FromBody] AddNewIncomeSourceCommand addNewIncomeSourceCommand)
        {
            return Created("", await _addNewIncomeSourceService.AddAsync(addNewIncomeSourceCommand));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListIncomeSourceResponse>>> GetAllAsync()
        {
            return Created("", await _incomeSourceQueryService.GetAllAsync());
        }
    }
}
