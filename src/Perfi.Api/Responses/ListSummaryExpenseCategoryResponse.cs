using Ardalis.GuardClauses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class ListSummaryExpenseCategoryResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<ListTransactionalExpenseCategoryResponse> Categories { get; set; }

        public static ListSummaryExpenseCategoryResponse From(SummaryExpenseCategory summaryExpenseCategory)
        {
            Guard.Against.Null(summaryExpenseCategory, nameof(summaryExpenseCategory));
            return new ListSummaryExpenseCategoryResponse
            {
                Code = summaryExpenseCategory.Code.Value,
                Name = summaryExpenseCategory.Name,
                Categories = ListTransactionalExpenseCategoryResponse.From(summaryExpenseCategory.Categories)
            };
        }
    }
}
