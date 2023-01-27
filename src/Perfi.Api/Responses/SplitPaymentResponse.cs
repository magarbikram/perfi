using Perfi.Core.Expenses;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Responses
{
    public class SplitPaymentResponse
    {
        public string SplitPartnerName { get; set; }
        public MoneyResponse OwnerShare { get; set; }
        public MoneyResponse SplitPartnerShare { get; set; }

        public static SplitPaymentResponse From(SplitPayment splitPayment, SplitPartner splitPartner)
        {
            return new SplitPaymentResponse
            {
                SplitPartnerName = splitPartner.Name,
                OwnerShare = MoneyResponse.From(splitPayment.OwnerShare),
                SplitPartnerShare = MoneyResponse.From(splitPayment.SplitPartnerShare)
            };
        }
    }
}