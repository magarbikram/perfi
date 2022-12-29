using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddNewIncomeSourceService
    {
        Task<NewIncomeSourceAddedResponse> AddAsync(AddNewIncomeSourceCommand addNewIncomeSourceCommand);
    }
}