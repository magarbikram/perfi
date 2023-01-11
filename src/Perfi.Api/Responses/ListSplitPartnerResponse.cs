using Ardalis.GuardClauses;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Responses
{
    public class ListSplitPartnerResponse
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        protected ListSplitPartnerResponse()
        {

        }

        public static ListSplitPartnerResponse From(SplitPartner splitPartner)
        {
            Guard.Against.Null(splitPartner);
            return new ListSplitPartnerResponse
            {
                Id = splitPartner.Id,
                Name = splitPartner.Name
            };
        }

        public static IEnumerable<ListSplitPartnerResponse> From(IEnumerable<SplitPartner> splitPartners)
        {
            return splitPartners.Select(sp => From(sp));
        }
    }
}
