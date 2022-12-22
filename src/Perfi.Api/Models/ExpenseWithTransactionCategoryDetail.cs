using Ardalis.GuardClauses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Models
{
    public class ExpenseWithTransactionCategoryDetail
    {
        public Expense Expense { get; private set; }
        public TransactionalExpenseCategory TransactionalExpenseCategory { get; private set; }

        public static ExpenseWithTransactionCategoryDetail From(Expense expense, TransactionalExpenseCategory transactionalExpenseCategory)
        {
            Guard.Against.Null(expense, nameof(expense));
            Guard.Against.Null(transactionalExpenseCategory, nameof(transactionalExpenseCategory));
            if (expense.ExpenseCategoryCode != transactionalExpenseCategory.Code)
            {
                throw new ArgumentException(message: $"transactionalExpenseCategory doesn't belong to expense");
            }
            return new ExpenseWithTransactionCategoryDetail
            {
                Expense = expense,
                TransactionalExpenseCategory = transactionalExpenseCategory
            };
        }
    }
}
