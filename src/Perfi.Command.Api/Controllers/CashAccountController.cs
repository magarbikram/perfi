using Microsoft.AspNetCore.Mvc;
using Perfi.Command.Api.Commands;
using Perfi.Command.Api.Responses;
using Perfi.Command.Api.Services;

namespace Perfi.Command.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashAccountController : ControllerBase
    {
        private readonly IAddCashAccountService _addCashAccountService;

        public CashAccountController(IAddCashAccountService addCashAccountService)
        {
            _addCashAccountService = addCashAccountService;
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddNewCashAccountCommand addNewCashAccountCommand)
        {
            NewCashAccountAddedResponse newCashAccountAddedResponse = await _addCashAccountService.AddAsync(addNewCashAccountCommand);
            return Created("", newCashAccountAddedResponse);
        }
    }
}
