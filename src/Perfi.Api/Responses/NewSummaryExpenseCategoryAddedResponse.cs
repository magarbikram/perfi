using Ardalis.GuardClauses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class NewSummaryExpenseCategoryAddedResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public static NewSummaryExpenseCategoryAddedResponse From(ExpenseCategory expenseCategory)
        {
            Guard.Against.Null(expenseCategory, nameof(expenseCategory));
            return new NewSummaryExpenseCategoryAddedResponse { Code = expenseCategory.Code.Value, Name = expenseCategory.Name };
        }
    }
}
