using Perfi.Api.Models;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.Core.Expenses.QueryModels;

namespace Perfi.Api.Services
{
    public class ExpenseQueryService : IExpenseQueryService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ITransactionalExpenseCategoryRepository _transactionalExpenseCategoryRepository;

        public ExpenseQueryService(IExpenseRepository expenseRepository, ITransactionalExpenseCategoryRepository transactionalExpenseCategoryRepository)
        {
            _expenseRepository = expenseRepository;
            _transactionalExpenseCategoryRepository = transactionalExpenseCategoryRepository;
        }
        public async Task<IEnumerable<ListExpenseResponse>> GetCurrentExpensesAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.For(DateTimeOffset.Now);
            IEnumerable<Expense> currentExpenses = await _expenseRepository.GetAllForTransactionPeriodAsync(transactionPeriod: currentTransactionPeriod);
            return await MapToResponsesAsync(currentExpenses);
        }

        private async Task<IEnumerable<ListExpenseResponse>> MapToResponsesAsync(IEnumerable<Expense> currentExpenses)
        {
            List<ExpenseWithTransactionCategoryDetail> expenseWithTransactionCategoryDetails = await PrepareExpenseWithTransactionCategoryDetails(currentExpenses);
            return ListExpenseResponse.From(expenseWithTransactionCategoryDetails);
        }

        private async Task<List<ExpenseWithTransactionCategoryDetail>> PrepareExpenseWithTransactionCategoryDetails(IEnumerable<Expense> currentExpenses)
        {
            ISet<ExpenseCategoryCode> expenseCategoryCodes = currentExpenses.Select(exp => exp.ExpenseCategoryCode).Distinct().ToHashSet();
            IEnumerable<TransactionalExpenseCategory> transactionalExpenseCategories = await _transactionalExpenseCategoryRepository.GetByCodesAsync(expenseCategoryCodes);
            IDictionary<ExpenseCategoryCode, TransactionalExpenseCategory> indexedTransactionalExpenseCategories =
                transactionalExpenseCategories.ToDictionary(tec => tec.Code);
            List<ExpenseWithTransactionCategoryDetail> expenseWithTransactionCategoryDetails = new();
            foreach (Expense expense in currentExpenses)
            {
                TransactionalExpenseCategory transactionalExpenseCategory = indexedTransactionalExpenseCategories[expense.ExpenseCategoryCode];
                ExpenseWithTransactionCategoryDetail expenseWithTransactionCategoryDetail = ExpenseWithTransactionCategoryDetail.From(expense, transactionalExpenseCategory);
                expenseWithTransactionCategoryDetails.Add(expenseWithTransactionCategoryDetail);
            }

            return expenseWithTransactionCategoryDetails;
        }

        public async Task<IEnumerable<ListExpenseResponse>> GetCurrentTop10ExpensesAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.For(DateTimeOffset.Now);
            IEnumerable<Expense> currentExpenses = await _expenseRepository.GetTop10ExpensesForTransactionPeriodAsync(transactionPeriod: currentTransactionPeriod);
            return await MapToResponsesAsync(currentExpenses);
        }

        public async Task<IEnumerable<ExpenseBySummaryCategoryResponse>> GetCurrentExpensesByCategoryAsync()
        {
            IEnumerable<ExpenseByCategory> expenseByCategories = await _expenseRepository.GetExpenseByCategoryAsync(TransactionPeriod.CurrentPeriod());
            return ExpenseBySummaryCategoryResponse.From(expenseByCategories);
        }
    }
}
