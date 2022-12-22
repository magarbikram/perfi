using Perfi.Api.Commands;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class NewExpenseAddedResponse
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public long TransactionDateUnixTimeStamp { get; private set; }
        public string ExpenseCategoryCode { get; private set; }
        public ExpensePaymentMethodResponse ExpensePaymentMethod { get; private set; }

        public static NewExpenseAddedResponse From(Expense expense)
        {
            return new NewExpenseAddedResponse
            {
                Id = expense.Id,
                Description = expense.Description,
                TransactionDateUnixTimeStamp = expense.TransactionDate.ToUnixTimeMilliseconds(),
                ExpenseCategoryCode = expense.ExpenseCategoryCode.Value,
                ExpensePaymentMethod = ExpensePaymentMethodResponse.From(expense.PaymentMethod)
            };
        }
    }
}
