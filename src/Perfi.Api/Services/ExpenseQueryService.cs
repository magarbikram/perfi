using Perfi.Api.Responses;
using Perfi.Api.Responses.Mappers;
using Perfi.Core.Expenses;
using Perfi.Core.Expenses.QueryModels;

namespace Perfi.Api.Services
{
    public class ExpenseQueryService : IExpenseQueryService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ListExpenseResponseMapper _listExpenseResponseMapper;

        public ExpenseQueryService(IExpenseRepository expenseRepository,
            ListExpenseResponseMapper listExpenseResponseMapper)
        {
            _expenseRepository = expenseRepository;
            _listExpenseResponseMapper = listExpenseResponseMapper;
        }
        public async Task<IEnumerable<ListExpenseResponse>> GetCurrentExpensesAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.CurrentPeriod();
            IEnumerable<Expense> currentExpenses = await _expenseRepository.GetAllForTransactionPeriodAsync(transactionPeriod: currentTransactionPeriod);
            return await MapToResponsesAsync(currentExpenses);
        }

        private async Task<IEnumerable<ListExpenseResponse>> MapToResponsesAsync(IEnumerable<Expense> currentExpenses)
        {
            return await _listExpenseResponseMapper.MapAsync(currentExpenses);
        }

        public async Task<IEnumerable<ListExpenseResponse>> GetCurrentTop10ExpensesAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.CurrentPeriod();
            IEnumerable<Expense> currentExpenses = await _expenseRepository.GetTop10ExpensesForTransactionPeriodAsync(transactionPeriod: currentTransactionPeriod);
            return await MapToResponsesAsync(currentExpenses);
        }

        public async Task<IEnumerable<ExpenseBySummaryCategoryResponse>> GetCurrentExpensesByCategoryAsync()
        {
            return await GetExpensesByCategoryAsync(TransactionPeriod.CurrentPeriod());
        }

        public async Task<IEnumerable<ExpenseBySummaryCategoryResponse>> GetExpensesByCategoryAsync(TransactionPeriod transactionPeriod)
        {
            IEnumerable<ExpenseByCategory> expenseByCategories = await _expenseRepository.GetExpenseByCategoryAsync(transactionPeriod);
            return ExpenseBySummaryCategoryResponse.From(expenseByCategories);
        }
    }
}
