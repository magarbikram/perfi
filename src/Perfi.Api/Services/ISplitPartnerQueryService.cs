using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface ISplitPartnerQueryService
    {
        Task<IEnumerable<ListSplitPartnerResponse>> GetAllAsync();
        Task<IEnumerable<ListSplitPartnerWithCurrentBalanceResponse>> GetAllWithCurrentBalanceAsync();
    }
}