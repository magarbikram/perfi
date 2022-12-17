using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class AddTransactionalExpenseCategoryService : IAddTransactionalExpenseCategoryService
    {
        private readonly ITransactionalExpenseCategoryRepository _transactionalExpenseCategoryRepository;
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly ISummaryExpenseCategoryRepository _summaryExpenseCategoryRepository;

        public AddTransactionalExpenseCategoryService(
            ITransactionalExpenseCategoryRepository transactionalExpenseCategoryRepository,
            ITransactionalAccountRepository transactionalAccountRepository,
            ISummaryExpenseCategoryRepository summaryExpenseCategoryRepository)
        {
            _transactionalExpenseCategoryRepository = transactionalExpenseCategoryRepository;
            _transactionalAccountRepository = transactionalAccountRepository;
            _summaryExpenseCategoryRepository = summaryExpenseCategoryRepository;
        }
        public async Task<NewTransactionalExpenseCategoryAddedResponse> AddAsync(AddNewTransactionalExpenseCategoryCommand addNewTransactionalExpenseCategoryCommand)
        {
            TransactionalAccount associatedExpenseAccount = await GetExpenseAccountAsync(addNewTransactionalExpenseCategoryCommand.ExpenseAccountNumber);
            SummaryExpenseCategory summaryExpenseCategory = await GetSummaryExpenseCategoryAsync(addNewTransactionalExpenseCategoryCommand.SummaryExpenseCategoryCode);
            TransactionalExpenseCategory transactionalExpenseCategory = TransactionalExpenseCategory.From(code: addNewTransactionalExpenseCategoryCommand.Code, name: addNewTransactionalExpenseCategoryCommand.Name, summaryExpenseCategory, associatedExpenseAccount);
            _transactionalExpenseCategoryRepository.Add(transactionalExpenseCategory);
            await _transactionalExpenseCategoryRepository.UnitOfWork.SaveChangesAsync();
            return NewTransactionalExpenseCategoryAddedResponse.From(transactionalExpenseCategory);
        }

        private async Task<SummaryExpenseCategory> GetSummaryExpenseCategoryAsync(string summaryExpenseCategoryCode)
        {
            Maybe<SummaryExpenseCategory> maybeSummaryExpenseCategory = await _summaryExpenseCategoryRepository.GetByCodeAsync(summaryExpenseCategoryCode);
            if (maybeSummaryExpenseCategory.HasNoValue)
            {
                throw new ResourceNotFoundException($"SummaryExpenseCategory with Code '{summaryExpenseCategoryCode}' not found");
            }
            return maybeSummaryExpenseCategory.Value;
        }

        private async Task<TransactionalAccount> GetExpenseAccountAsync(string expenseAccountNumber)
        {
            Maybe<TransactionalAccount> expenseAccount = await _transactionalAccountRepository.GetHomeExpenseAccountByNumberAsync(expenseAccountNumber);
            if (expenseAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Expense account with account number '{expenseAccountNumber}' not found");
            }
            return expenseAccount.Value;
        }
    }
}
