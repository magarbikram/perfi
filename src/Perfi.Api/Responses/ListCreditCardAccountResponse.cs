using Ardalis.GuardClauses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.CreditCardAggregate;

namespace Perfi.Api.Responses
{
    public class ListCreditCardAccountResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreditorName { get; set; }
        public string LastFourDigits { get; set; }
        public MoneyResponse CurrentBalance { get; set; }

        public static ListCreditCardAccountResponse From(CreditCardAccount creditCardAccount)
        {
            Guard.Against.Null(creditCardAccount, nameof(creditCardAccount));
            return new ListCreditCardAccountResponse { Id = creditCardAccount.Id, Name = creditCardAccount.Name, CreditorName = creditCardAccount.CreditorName, LastFourDigits = creditCardAccount.LastFourDigits };
        }

        public static ListCreditCardAccountResponse From(CreditCardAccount creditCardAccount, Money currentBalance)
        {
            ListCreditCardAccountResponse response = From(creditCardAccount);
            response.CurrentBalance = MoneyResponse.From(currentBalance);
            return response;
        }
    }
}
