using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddExpenseAccountService
    {
        Task<NewExpenseAccountAddedResponse> AddAsync(AddNewExpenseAccountCommand addNewExpenseAccountCommand);
    }
}