using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Core.Expenses
{
    public class TransactionalExpenseCategory : ExpenseCategory
    {
        public ExpenseCategoryCode SummaryExpenseCategoryCode { get; set; }

        public static TransactionalExpenseCategory From(string code, string name, SummaryExpenseCategory summaryExpenseCategory, TransactionalAccount associatedExpenseAccount)
        {
            ValidateName(name);
            ValidateAssocaitedExpenseAccount(associatedExpenseAccount);
            return new TransactionalExpenseCategory
            {
                Code = ExpenseCategoryCode.From(code),
                Name = name,
                SummaryExpenseCategoryCode = ExpenseCategoryCode.From(summaryExpenseCategory.Code.Value),
                AssociatedExpenseAccountNumber = associatedExpenseAccount.Number
            };
        }
    }
}
