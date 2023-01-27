using Perfi.Core.Accounting;
using Perfi.Core.Expenses.QueryModels;

namespace Perfi.Api.Responses
{
    public class ExpenseBySummaryCategoryResponse
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public MoneyResponse TotalExpenseAmount { get; set; }

        public IEnumerable<ExpenseByTransactionalExpenseCategoryResponse> TransactionalCategories { get; set; }

        public static IEnumerable<ExpenseBySummaryCategoryResponse> From(IEnumerable<ExpenseByCategory> expenseByCategories)
        {
            List<ExpenseBySummaryCategoryResponse> summaryExpenseCategoryTotalResponses = new();
            var groupedExpenseByCategories = expenseByCategories.GroupBy(e => new { e.ParentExpenseCategoryCode, e.ParentExpenseCategoryName });
            foreach (var expenseByParentCategory in groupedExpenseByCategories)
            {
                ExpenseBySummaryCategoryResponse summaryExpenseCategoryTotalResponse = new ExpenseBySummaryCategoryResponse
                {
                    CategoryCode = expenseByParentCategory.Key.ParentExpenseCategoryCode,
                    CategoryName = expenseByParentCategory.Key.ParentExpenseCategoryName,
                    TotalExpenseAmount = MoneyResponse.From(Money.Sum(expenseByParentCategory.Select(x => x.ExpenseAmount))),
                    TransactionalCategories = MapToExpenseByTransactionalCategories(expenseByParentCategory)
                };
                summaryExpenseCategoryTotalResponses.Add(summaryExpenseCategoryTotalResponse);
            }
            return summaryExpenseCategoryTotalResponses;
        }

        private static IEnumerable<ExpenseByTransactionalExpenseCategoryResponse> MapToExpenseByTransactionalCategories(IEnumerable<ExpenseByCategory> expenseByTransactionalCategories)
        {
            List<ExpenseByTransactionalExpenseCategoryResponse> transactionalExpenseCategoryTotalResponses = new List<ExpenseByTransactionalExpenseCategoryResponse>();
            foreach (ExpenseByCategory expenseByTransactionalCategory in expenseByTransactionalCategories)
            {
                transactionalExpenseCategoryTotalResponses.Add(MapToExpenseByTransactionalCategory(expenseByTransactionalCategory));
            }
            return transactionalExpenseCategoryTotalResponses;
        }

        private static ExpenseByTransactionalExpenseCategoryResponse MapToExpenseByTransactionalCategory(ExpenseByCategory expenseByTransactionalCategory)
        {
            return new ExpenseByTransactionalExpenseCategoryResponse
            {
                CategoryCode = expenseByTransactionalCategory.ExpenseCategoryCode,
                CategoryName = expenseByTransactionalCategory.ExpenseCategoryName,
                TotalExpenseAmount = MoneyResponse.From(expenseByTransactionalCategory.ExpenseAmount)
            };
        }
    }
}
