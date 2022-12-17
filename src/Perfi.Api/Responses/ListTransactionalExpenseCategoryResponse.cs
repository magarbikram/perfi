using Ardalis.GuardClauses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class ListTransactionalExpenseCategoryResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public static ListTransactionalExpenseCategoryResponse From(TransactionalExpenseCategory transactionalExpenseCategory)
        {
            Guard.Against.Null(transactionalExpenseCategory, nameof(transactionalExpenseCategory));
            return new ListTransactionalExpenseCategoryResponse { Code = transactionalExpenseCategory.Code.Value, Name = transactionalExpenseCategory.Name };
        }

        public static List<ListTransactionalExpenseCategoryResponse> From(IEnumerable<TransactionalExpenseCategory> categories)
        {
            List<ListTransactionalExpenseCategoryResponse> responseCategories = new List<ListTransactionalExpenseCategoryResponse>();
            foreach (TransactionalExpenseCategory category in categories)
            {
                responseCategories.Add(ListTransactionalExpenseCategoryResponse.From(category));
            }
            return responseCategories;
        }
    }
}
