using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddCashAccountService
    {
        Task<NewCashAccountAddedResponse> AddAsync(AddNewCashAccountCommand addNewCashAccountCommand);
    }
}