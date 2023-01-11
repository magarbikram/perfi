using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SplitPartnersController : ControllerBase
    {
        private readonly IAddSplitPartnerService _addSplitPartnerService;
        private readonly ISplitPartnerQueryService _splitPartnerQueryService;

        public SplitPartnersController(
            IAddSplitPartnerService addSplitPartnerService,
            ISplitPartnerQueryService splitPartnerQueryService)
        {
            _addSplitPartnerService = addSplitPartnerService;
            _splitPartnerQueryService = splitPartnerQueryService;
        }
        [HttpPost]
        public async Task<ActionResult<NewSplitPartnerAddedResponse>> AddAsync([FromBody] AddNewSplitParnerCommand addNewSplitParnerCommand)
        {
            return Created("", await _addSplitPartnerService.AddAsync(addNewSplitParnerCommand));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListSplitPartnerResponse>>> GetAllAsync()
        {
            return Ok(await _splitPartnerQueryService.GetAllAsync());
        }

        [HttpGet("WithCurrentBalance")]
        public async Task<ActionResult<IEnumerable<ListSplitPartnerResponse>>> GetAllWithCurrentBalanceAsync()
        {
            return Ok(await _splitPartnerQueryService.GetAllWithCurrentBalanceAsync());
        }
    }
}
