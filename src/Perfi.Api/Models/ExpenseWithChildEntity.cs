using Ardalis.GuardClauses;
using Perfi.Core.Expenses;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Models
{
    public class ExpenseWithChildEntity
    {
        public Expense Expense { get; private set; }
        public TransactionalExpenseCategory TransactionalExpenseCategory { get; private set; }
        public SplitPartner SplitPartner { get; private set; }

        public static ExpenseWithChildEntity From(Expense expense, TransactionalExpenseCategory transactionalExpenseCategory)
        {
            Guard.Against.Null(expense, nameof(expense));
            Guard.Against.Null(transactionalExpenseCategory, nameof(transactionalExpenseCategory));
            if (expense.ExpenseCategoryCode != transactionalExpenseCategory.Code)
            {
                throw new ArgumentException(message: $"transactionalExpenseCategory doesn't belong to expense");
            }
            return new ExpenseWithChildEntity
            {
                Expense = expense,
                TransactionalExpenseCategory = transactionalExpenseCategory
            };
        }

        public void SetSplitPartner(SplitPartner splitPartner)
        {
            SplitPartner = splitPartner;
        }
    }
}
