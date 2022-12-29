using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Core.Earnings
{
    public class PaymenDepositionToCashAccount : PaymentDeposition
    {
        public string Name { get; private set; }
        public string BankName { get; private set; }
        public AccountNumber CashAccountNumber { get; private set; }
        public int CashAccountId { get; private set; }

        protected PaymenDepositionToCashAccount()
        {

        }

        public static PaymenDepositionToCashAccount From(CashAccount cashAccount)
        {
            Guard.Against.Null(cashAccount, nameof(cashAccount));
            return new PaymenDepositionToCashAccount
            {
                CashAccountId = cashAccount.Id,
                Name = cashAccount.Name,
                BankName = cashAccount.BankName,
                CashAccountNumber = cashAccount.AssociatedAccountNumber
            };
        }

        public override AccountNumber GetAssociatedAccountNumber()
        {
            return CashAccountNumber;
        }
    }
}
