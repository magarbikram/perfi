using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class AddSummaryExpenseCategoryService : IAddSummaryExpenseCategoryService
    {
        private readonly ISummaryExpenseCategoryRepository _summaryExpenseCategoryRepository;
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;

        public AddSummaryExpenseCategoryService(
            ISummaryExpenseCategoryRepository summaryExpenseCategoryRepository,
            ITransactionalAccountRepository transactionalAccountRepository)
        {
            _summaryExpenseCategoryRepository = summaryExpenseCategoryRepository;
            _transactionalAccountRepository = transactionalAccountRepository;
        }
        public async Task<NewSummaryExpenseCategoryAddedResponse> AddAsync(AddNewSummaryExpenseCategoryCommand addNewSummaryExpenseCategoryCommand)
        {
            TransactionalAccount associatedExpenseAccount = await GetExpenseAccountAsync(addNewSummaryExpenseCategoryCommand.ExpenseAccountNumber);
            SummaryExpenseCategory summaryExpenseCategory = SummaryExpenseCategory.From(code: addNewSummaryExpenseCategoryCommand.Code, name: addNewSummaryExpenseCategoryCommand.Name, associatedExpenseAccount);
            _summaryExpenseCategoryRepository.Add(summaryExpenseCategory);
            await _summaryExpenseCategoryRepository.UnitOfWork.SaveChangesAsync();
            return NewSummaryExpenseCategoryAddedResponse.From(summaryExpenseCategory);
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
