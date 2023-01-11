using Microsoft.EntityFrameworkCore.Query;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class SplitPartnerPaymentMethodResponse
    {
        public string SplitPartnerName { get; private set; }
        public int SplitPartnerId { get; private set; }
        public static SplitPartnerPaymentMethodResponse From(SplitPartnerExpensePaymentMethod splitPartnerExpensePaymentMethod)
        {
            return new SplitPartnerPaymentMethodResponse
            {
                SplitPartnerName = splitPartnerExpensePaymentMethod.SplitPartnerName,
                SplitPartnerId = splitPartnerExpensePaymentMethod.SplitPartnerId
            };
        }
    }
}
