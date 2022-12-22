using CSharpFunctionalExtensions;
using Perfi.SharedKernel;

namespace Perfi.Core.Expenses
{
    public interface ITransactionalExpenseCategoryRepository : IRepository<TransactionalExpenseCategory>
    {
        TransactionalExpenseCategory Add(TransactionalExpenseCategory category);
        Task<Maybe<TransactionalExpenseCategory>> GetByCodeAsync(ExpenseCategoryCode expenseCategoryCode);
        Task<IEnumerable<TransactionalExpenseCategory>> GetByCodesAsync(IEnumerable<ExpenseCategoryCode> expenseCategoryCodes);
    }
}
