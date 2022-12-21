using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class ListExpenseResponse
    {
        public string Description { get; private set; }
        public long TransactionDateUnixTimeStamp { get; private set; }
        public string ExpenseCategoryCode { get; private set; }
        public ExpensePaymentMethodResponse ExpensePaymentMethod { get; private set; }

        public static ListExpenseResponse From(Expense expense)
        {
            return new ListExpenseResponse
            {
                Description = expense.Description,
                TransactionDateUnixTimeStamp = expense.TransactionDate.ToUnixTimeMilliseconds(),
                ExpenseCategoryCode = expense.ExpenseCategoryCode.Value,
                ExpensePaymentMethod = ExpensePaymentMethodResponse.From(expense.PaymentMethod)
            };
        }

        public static List<ListExpenseResponse> From(IEnumerable<Expense> currentExpenses)
        {
            List<ListExpenseResponse> listExpenseResponses = new(currentExpenses.Count());
            foreach (Expense expense in currentExpenses)
            {
                listExpenseResponses.Add(From(expense));
            }
            return listExpenseResponses;
        }
    }
}
