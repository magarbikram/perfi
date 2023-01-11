using CSharpFunctionalExtensions;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.SplitPartners;

namespace Perfi.Core.Expenses
{
    public class SplitPayment : ValueObject
    {
        public int SplitPartnerId { get; private set; }
        public AccountNumber SplitPartnerReceivableAccountNumber { get; private set; }
        public Money OwnerShare { get; private set; }
        public Money SplitPartnerShare { get; private set; }

        protected SplitPayment()
        {

        }
        public static SplitPayment From(SplitPartner splitPartner, Money ownerShare, Money splitPartnerShareExpenseAmount)
        {
            return new SplitPayment
            {
                SplitPartnerId = splitPartner.Id,
                SplitPartnerReceivableAccountNumber = splitPartner.ReceivableAccountNumber,
                OwnerShare = ownerShare,
                SplitPartnerShare = splitPartnerShareExpenseAmount
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OwnerShare;
            yield return SplitPartnerShare;
        }

        public Money GetTotalAmount()
        {
            return OwnerShare + SplitPartnerShare;
        }
    }
}
