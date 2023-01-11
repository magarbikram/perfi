using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IAddSplitPartnerService
    {
        Task<NewSplitPartnerAddedResponse> AddAsync(AddNewSplitParnerCommand addNewSplitParnerCommand);
    }
}