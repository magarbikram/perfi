using Perfi.Api.Models;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class ListExpenseResponse
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public long TransactionDateUnixTimeStamp { get; private set; }
        public ListTransactionalExpenseCategoryResponse ExpenseCategory { get; private set; }
        public ExpensePaymentMethodResponse ExpensePaymentMethod { get; private set; }
        public MoneyResponse Amount { get; private set; }
        public static ListExpenseResponse From(ExpenseWithTransactionCategoryDetail expenseWithTransactionCategoryDetail)
        {
            Expense expense = expenseWithTransactionCategoryDetail.Expense;
            TransactionalExpenseCategory transactionalExpenseCategory = expenseWithTransactionCategoryDetail.TransactionalExpenseCategory;
            return new ListExpenseResponse
            {
                Id = expense.Id,
                Description = expense.Description,
                TransactionDateUnixTimeStamp = expense.TransactionDate.ToUnixTimeMilliseconds(),
                ExpenseCategory = ListTransactionalExpenseCategoryResponse.From(transactionalExpenseCategory),
                ExpensePaymentMethod = ExpensePaymentMethodResponse.From(expense.PaymentMethod),
                Amount = MoneyResponse.From(expense.Amount),
            };
        }

        internal static IEnumerable<ListExpenseResponse> From(IEnumerable<ExpenseWithTransactionCategoryDetail> expenseWithTransactionCategoryDetails)
        {
            List<ListExpenseResponse> listExpenseResponses = new(expenseWithTransactionCategoryDetails.Count());
            foreach (ExpenseWithTransactionCategoryDetail expenseWithTransactionCategoryDetail in expenseWithTransactionCategoryDetails)
            {
                listExpenseResponses.Add(From(expenseWithTransactionCategoryDetail));
            }
            return listExpenseResponses;
        }
    }
}
