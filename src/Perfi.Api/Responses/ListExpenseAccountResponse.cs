using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Api.Responses
{
    public class ListExpenseAccountResponse
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public static ListExpenseAccountResponse From(TransactionalAccount expenseAccount)
        {
            Guard.Against.Null(expenseAccount, nameof(expenseAccount));
            return new ListExpenseAccountResponse { Number = expenseAccount.Number.Value, Name = expenseAccount.Name };
        }
    }
}
