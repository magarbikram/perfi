using Ardalis.GuardClauses;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Responses
{
    public class NewSplitPartnerAddedResponse
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        protected NewSplitPartnerAddedResponse()
        {

        }

        public static NewSplitPartnerAddedResponse From(SplitPartner splitPartner)
        {
            Guard.Against.Null(splitPartner);
            return new NewSplitPartnerAddedResponse
            {
                Id = splitPartner.Id,
                Name = splitPartner.Name
            };
        }
    }
}
