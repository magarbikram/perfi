using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Core.Expenses
{
    public class SummaryExpenseCategory : ExpenseCategory
    {
        public IEnumerable<TransactionalExpenseCategory> Categories { get; } = new List<TransactionalExpenseCategory>();

        public static SummaryExpenseCategory From(string code, string name, TransactionalAccount associatedExpenseAccount)
        {
            ValidateName(name);
            ValidateAssocaitedExpenseAccount(associatedExpenseAccount);
            return new SummaryExpenseCategory
            {
                Code = ExpenseCategoryCode.From(code),
                Name = name,
                AssociatedExpenseAccountNumber = associatedExpenseAccount.Number,
            };
        }
    }
}
