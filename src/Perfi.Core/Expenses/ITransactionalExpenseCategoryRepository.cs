using Perfi.SharedKernel;

namespace Perfi.Core.Expenses
{
    public interface ITransactionalExpenseCategoryRepository : IRepository<TransactionalExpenseCategory>
    {
        TransactionalExpenseCategory Add(TransactionalExpenseCategory category);
    }
}
