using Ardalis.GuardClauses;
using Perfi.Core.Expenses;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Responses
{
    public class NewTransactionalExpenseCategoryAddedResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SummaryExpenseCategoryCode { get; set; }

        public static NewTransactionalExpenseCategoryAddedResponse From(TransactionalExpenseCategory transactionalExpenseCategory)
        {
            Guard.Against.Null(transactionalExpenseCategory, nameof(transactionalExpenseCategory));
            return new NewTransactionalExpenseCategoryAddedResponse { Code = transactionalExpenseCategory.Code.Value, Name = transactionalExpenseCategory.Name, SummaryExpenseCategoryCode = transactionalExpenseCategory.SummaryExpenseCategoryCode.Value };
        }
    }
}
