using Ardalis.GuardClauses;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Command.Api.Responses
{
    public class NewCashAccountAddedResponse
    {
        public string Name { get; set; }
        public string BankName { get; set; }
        public static NewCashAccountAddedResponse From(CashAccount cashAccount)
        {
            Guard.Against.Null(cashAccount, nameof(cashAccount));
            return new NewCashAccountAddedResponse { Name = cashAccount.Name, BankName = cashAccount.BankName };
        }
    }
}
