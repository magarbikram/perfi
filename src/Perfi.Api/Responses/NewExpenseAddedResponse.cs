using Perfi.Api.Commands;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class NewExpenseAddedResponse
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public long TransactionDateUnixTimeStamp { get; private set; }
        public ListTransactionalExpenseCategoryResponse ExpenseCategory { get; private set; }
        public ExpensePaymentMethodResponse ExpensePaymentMethod { get; private set; }
        public MoneyResponse Amount { get; private set; }

        public static NewExpenseAddedResponse From(Expense expense, TransactionalExpenseCategory transactionalExpenseCategory)
        {
            return new NewExpenseAddedResponse
            {
                Id = expense.Id,
                Description = expense.Description,
                TransactionDateUnixTimeStamp = expense.TransactionDate.ToUnixTimeMilliseconds(),
                ExpenseCategory = ListTransactionalExpenseCategoryResponse.From(transactionalExpenseCategory),
                ExpensePaymentMethod = ExpensePaymentMethodResponse.From(expense.PaymentMethod),
                Amount = MoneyResponse.From(expense.Amount)
            };
        }
    }
}
