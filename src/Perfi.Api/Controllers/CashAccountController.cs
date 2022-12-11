using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
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
