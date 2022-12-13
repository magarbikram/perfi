using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddLoanService
    {
        Task<NewLoanAddedResponse> AddAsync(AddNewLoanCommand addNewLoanCommand);
    }
}