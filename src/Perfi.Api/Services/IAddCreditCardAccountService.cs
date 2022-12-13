using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddCreditCardAccountService
    {
        Task<NewCreditCardAccountAddedResponse> AddAsync(AddNewCreditCardAccountCommand addNewCreditCardAccountCommand);
    }
}