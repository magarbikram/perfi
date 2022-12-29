using Perfi.Core.Accounting;
using Perfi.SharedKernel;

namespace Perfi.Core.Expenses
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Expense Add(Expense expense);
        Task<Money> GetTotalExpenseAmountForPeriodAsync(TransactionPeriod currentTransactionPeriod);
        Task<IEnumerable<Expense>> GetAllForTransactionPeriodAsync(TransactionPeriod transactionPeriod);
        Task<IEnumerable<Expense>> GetTop10ExpensesForTransactionPeriodAsync(TransactionPeriod transactionPeriod);
    }
}
