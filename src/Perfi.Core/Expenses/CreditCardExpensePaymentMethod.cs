using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;

namespace Perfi.Core.Expenses
{
    public class CreditCardExpensePaymentMethod : ExpensePaymentMethod
    {
        public string Name { get; private set; }
        public string LastFourDigits { get; private set; }
        public string CreditorName { get; private set; }
        public AccountNumber AssociatedAccountNumber { get; private set; }

        protected CreditCardExpensePaymentMethod()
        {

        }
        public static CreditCardExpensePaymentMethod From(CreditCardAccount creditCardAccount)
        {
            return new CreditCardExpensePaymentMethod
            {
                Name = creditCardAccount.Name,
                LastFourDigits = creditCardAccount.LastFourDigits,
                CreditorName = creditCardAccount.CreditorName,
                AssociatedAccountNumber = creditCardAccount.AssociatedAccountNumber
            };
        }

        public override AccountNumber GetAssociatedAccountNumber()
        {
            return AssociatedAccountNumber;
        }
    }
}