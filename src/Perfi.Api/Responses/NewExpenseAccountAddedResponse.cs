using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Api.Responses
{
    public class NewExpenseAccountAddedResponse
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public static NewExpenseAccountAddedResponse From(TransactionalAccount expenseAccount)
        {
            Guard.Against.Null(expenseAccount, nameof(expenseAccount));
            return new NewExpenseAccountAddedResponse { Number = expenseAccount.Number.Value, Name = expenseAccount.Name };
        }
    }
}
