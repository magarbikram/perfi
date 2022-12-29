using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddNewIncomeService
    {
        Task<NewIncomeAddedResponse> AddAsync(AddNewIncomeCommand addJobIncomeCommand);
    }
}