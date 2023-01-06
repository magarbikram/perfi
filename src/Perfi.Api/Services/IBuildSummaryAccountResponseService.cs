using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IBuildSummaryAccountResponseService
    {
        Task<AccountSummaryResponse> Build();
    }
}