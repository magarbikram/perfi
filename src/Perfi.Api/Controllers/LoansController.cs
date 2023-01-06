using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IAddLoanService _addLoanService;
        private readonly ILoanQueryService _cashAccountQueryService;

        public LoansController(
            IAddLoanService addLoanService,
            ILoanQueryService cashAccountQueryService)
        {
            _addLoanService = addLoanService;
            _cashAccountQueryService = cashAccountQueryService;
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
            List<ListLoanResponse> listLoanResponses = await _cashAccountQueryService.GetAllAsync(withCurrentBalance);
            return Ok(listLoanResponses);
        }
    }
}
