using Ardalis.GuardClauses;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;

namespace Perfi.Api.Responses
{
    public class NewCreditCardAccountAddedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreditorName { get; set; }
        public string LastFourDigits { get; set; }
        public static NewCreditCardAccountAddedResponse From(CreditCardAccount creditCardAccount)
        {
            Guard.Against.Null(creditCardAccount, nameof(creditCardAccount));
            return new NewCreditCardAccountAddedResponse { Id = creditCardAccount.Id, Name = creditCardAccount.Name, CreditorName = creditCardAccount.CreditorName, LastFourDigits = creditCardAccount.LastFourDigits };
        }
    }
}
