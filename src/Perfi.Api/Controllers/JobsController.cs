using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IAddJobService _addJobService;

        public JobsController(IAddJobService addJobService)
        {
            _addJobService = addJobService;
        }

        [HttpPost]
        public async Task<ActionResult<NewJobAddedResponse>> AddNewJobAsync([FromBody] AddNewJobCommand addNewJobCommand)
        {
            return Created("", await _addJobService.AddAsync(addNewJobCommand));
        }
    }
}
