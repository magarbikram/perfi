using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddNewExpenseService
    {
        Task<NewExpenseAddedResponse> AddAsync(AddNewExpenseCommand addNewExpenseCommand);
    }
}