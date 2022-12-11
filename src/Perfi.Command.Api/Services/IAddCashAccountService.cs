using Perfi.Command.Api.Commands;
using Perfi.Command.Api.Responses;

namespace Perfi.Command.Api.Services
{
    public interface IAddCashAccountService
    {
        Task<NewCashAccountAddedResponse> AddAsync(AddNewCashAccountCommand addNewCashAccountCommand);
    }
}