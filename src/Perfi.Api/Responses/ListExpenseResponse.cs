using Perfi.Api.Commands;
using Perfi.Api.Models;
using Perfi.Core.Expenses;
using Perfi.Core.SplitPartners;

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
        public SplitPaymentResponse SplitPayment { get; private set; }
        public static ListExpenseResponse From(ExpenseWithChildEntity expenseWithTransactionCategoryDetail)
        {
            Expense expense = expenseWithTransactionCategoryDetail.Expense;
            TransactionalExpenseCategory transactionalExpenseCategory = expenseWithTransactionCategoryDetail.TransactionalExpenseCategory;
            ListExpenseResponse listExpenseResponse = new ListExpenseResponse
            {
                Id = expense.Id,
                Description = expense.Description,
                TransactionDateUnixTimeStamp = expense.TransactionDate.ToUnixTimeMilliseconds(),
                ExpenseCategory = ListTransactionalExpenseCategoryResponse.From(transactionalExpenseCategory),
                ExpensePaymentMethod = ExpensePaymentMethodResponse.From(expense.PaymentMethod),
                Amount = MoneyResponse.From(expense.Amount)
            };
            if (expense.IsSplit())
            {
                SplitPartner splitPartner = expenseWithTransactionCategoryDetail.SplitPartner;
                listExpenseResponse.SplitPayment = SplitPaymentResponse.From(expense.SplitPayment!, splitPartner);
            }
            return listExpenseResponse;
        }

        internal static List<ListExpenseResponse> From(IEnumerable<ExpenseWithChildEntity> expenseWithTransactionCategoryDetails)
        {
            List<ListExpenseResponse> listExpenseResponses = new(expenseWithTransactionCategoryDetails.Count());
            foreach (ExpenseWithChildEntity expenseWithTransactionCategoryDetail in expenseWithTransactionCategoryDetails)
            {
                listExpenseResponses.Add(From(expenseWithTransactionCategoryDetail));
            }
            return listExpenseResponses;
        }
    }
}
