using Ardalis.GuardClauses;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class CreditCardPaymentMethodResponse
    {
        public string Name { get; set; }
        public string CreditorName { get; set; }
        public string LastFourDigits { get; set; }
        public static CreditCardPaymentMethodResponse From(CreditCardAccount creditCardAccount)
        {
            Guard.Against.Null(creditCardAccount, nameof(creditCardAccount));
            return new CreditCardPaymentMethodResponse { Name = creditCardAccount.Name, CreditorName = creditCardAccount.CreditorName, LastFourDigits = creditCardAccount.LastFourDigits };
        }

        public static CreditCardPaymentMethodResponse From(CreditCardExpensePaymentMethod creditCardExpensePaymentMethod)
        {
            Guard.Against.Null(creditCardExpensePaymentMethod, nameof(creditCardExpensePaymentMethod));
            return new CreditCardPaymentMethodResponse { Name = creditCardExpensePaymentMethod.Name, CreditorName = creditCardExpensePaymentMethod.CreditorName, LastFourDigits = creditCardExpensePaymentMethod.LastFourDigits };
        }
    }
}
