using Ardalis.GuardClauses;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Api.Responses
{
    public class ListCashAccountResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public MoneyResponse CurrentBalance { get; set; }


        public static ListCashAccountResponse From(CashAccount cashAccount)
        {
            Guard.Against.Null(cashAccount, nameof(cashAccount));
            ListCashAccountResponse response = new ListCashAccountResponse { Id = cashAccount.Id, Name = cashAccount.Name, BankName = cashAccount.BankName };
            return response;
        }

        public static ListCashAccountResponse From(CashAccount cashAccount, Core.Accounting.Money currentBalance)
        {
            Guard.Against.Null(currentBalance);
            ListCashAccountResponse response = From(cashAccount);
            response.CurrentBalance = MoneyResponse.From(currentBalance);
            return response;
        }
    }
}
