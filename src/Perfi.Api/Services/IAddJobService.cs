using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddJobService
    {
        Task<NewJobAddedResponse> AddAsync(AddNewJobCommand addNewJobCommand);
    }
}