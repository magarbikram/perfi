using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.SplitPartners;

namespace Perfi.Core.Expenses
{
    public class SplitPartnerExpensePaymentMethod : ExpensePaymentMethod
    {
        public string SplitPartnerName { get; private set; }
        public int SplitPartnerId { get; private set; }
        public AccountNumber ReceivableAccountNumber { get; private set; }
        public static SplitPartnerExpensePaymentMethod From(SplitPartner splitPartner)
        {
            return new()
            {
                SplitPartnerName = splitPartner.Name,
                SplitPartnerId = splitPartner.Id,
                ReceivableAccountNumber = splitPartner.ReceivableAccountNumber
            };
        }
        public override AccountNumber GetAssociatedAccountNumber()
        {
            return ReceivableAccountNumber;
        }
    }
}