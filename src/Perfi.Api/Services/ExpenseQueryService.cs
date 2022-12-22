using Perfi.Api.Responses;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class ExpenseQueryService : IExpenseQueryService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseQueryService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
        public async Task<IEnumerable<ListExpenseResponse>> GetCurrentExpensesAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.For(DateTimeOffset.Now);
            IEnumerable<Expense> currentExpenses = await _expenseRepository.GetAllForTransactionPeriodAsync(transactionPeriod: currentTransactionPeriod);
            return ListExpenseResponse.From(currentExpenses);
        }

        public async Task<IEnumerable<ListExpenseResponse>> GetCurrentTop10ExpensesAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.For(DateTimeOffset.Now);
            IEnumerable<Expense> currentExpenses = await _expenseRepository.GetTop10ExpensesForTransactionPeriodAsync(transactionPeriod: currentTransactionPeriod);
            return ListExpenseResponse.From(currentExpenses);
        }
    }
}
