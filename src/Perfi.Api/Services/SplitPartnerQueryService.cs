using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Services
{
    public class SplitPartnerQueryService : ISplitPartnerQueryService
    {
        private readonly ISplitPartnerRepository _splitPartnerRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;

        public SplitPartnerQueryService(
            ISplitPartnerRepository splitPartnerRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService)
        {
            _splitPartnerRepository = splitPartnerRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
        }
        public async Task<IEnumerable<ListSplitPartnerResponse>> GetAllAsync()
        {
            IEnumerable<SplitPartner> splitPartners = await _splitPartnerRepository.GetAllAsync();
            return ListSplitPartnerResponse.From(splitPartners);
        }

        public async Task<IEnumerable<ListSplitPartnerWithCurrentBalanceResponse>> GetAllWithCurrentBalanceAsync()
        {
            IEnumerable<SplitPartner> splitPartners = await _splitPartnerRepository.GetAllAsync();
            return await ListSplitPartnerWithCurrentBalanceResponse.FromAsync(splitPartners, _calculateCurrentBalanceService);
        }
    }
}
