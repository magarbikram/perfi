using Microsoft.AspNetCore.Mvc;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Api.Services;

namespace Perfi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionalExpenseCategoriesController : ControllerBase
    {
        private readonly IAddTransactionalExpenseCategoryService _addTransactionalExpenseCategoryService;

        public TransactionalExpenseCategoriesController(
            IAddTransactionalExpenseCategoryService addTransactionalExpenseCategoryService)
        {
            _addTransactionalExpenseCategoryService = addTransactionalExpenseCategoryService;
        }

        [HttpPost]
        public async Task<ActionResult<NewTransactionalExpenseCategoryAddedResponse>> AddAsync([FromBody] AddNewTransactionalExpenseCategoryCommand addTransactionalExpenseCategoryCommand)
        {
            NewTransactionalExpenseCategoryAddedResponse newTransactionalExpenseCategoryAddedResponse = await _addTransactionalExpenseCategoryService.AddAsync(addTransactionalExpenseCategoryCommand);
            return Created("", newTransactionalExpenseCategoryAddedResponse);
        }
    }
}
