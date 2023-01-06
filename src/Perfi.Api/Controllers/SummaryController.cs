using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly IBuildSummaryAccountResponseService _buildSummaryAccountResponseService;

        public SummaryController(IBuildSummaryAccountResponseService buildSummaryAccountResponseService)
        {
            _buildSummaryAccountResponseService = buildSummaryAccountResponseService;
        }

        [HttpGet("AccountsSummary")]
        public async Task<ActionResult<AccountSummaryResponse>> GetSummaryAccountAsync()
        {
            return Ok(await _buildSummaryAccountResponseService.Build());
        }
    }
}
