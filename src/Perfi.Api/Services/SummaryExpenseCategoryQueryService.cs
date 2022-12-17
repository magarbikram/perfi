using CSharpFunctionalExtensions;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class SummaryExpenseCategoryQueryService : ISummaryExpenseCategoryQueryService
    {
        private readonly ISummaryExpenseCategoryRepository _summaryExpenseCategoryRepository;
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;

        public SummaryExpenseCategoryQueryService(
            ISummaryExpenseCategoryRepository summaryExpenseCategoryRepository,
            ITransactionalAccountRepository transactionalAccountRepository)
        {
            _summaryExpenseCategoryRepository = summaryExpenseCategoryRepository;
            _transactionalAccountRepository = transactionalAccountRepository;
        }

        public async Task<IEnumerable<ListSummaryExpenseCategoryResponse>> GetAllAsync()
        {
            IEnumerable<SummaryExpenseCategory> summaryExpenseCategories = await _summaryExpenseCategoryRepository.GetAllAsync(includeCategories: true);
            List<ListSummaryExpenseCategoryResponse> listSummaryExpenseCategoryResponses = new List<ListSummaryExpenseCategoryResponse>();
            foreach (SummaryExpenseCategory summaryExpenseCategory in summaryExpenseCategories)
            {
                listSummaryExpenseCategoryResponses.Add(ListSummaryExpenseCategoryResponse.From(summaryExpenseCategory));
            }
            return listSummaryExpenseCategoryResponses;
        }

        public async Task<ListExpenseAccountResponse> GetAssociatedExpenseAccountAsync(string summaryExpenseCategoryCode)
        {
            SummaryExpenseCategory summaryExpenseCategory = await FindSummaryExpenseCategoryByCodeAsync(summaryExpenseCategoryCode);
            string associatedExpenseAccountNumber = summaryExpenseCategory.AssociatedExpenseAccountNumber.Value;
            TransactionalAccount expenseAccount = await FindExpenseAccountByNumberAsync(associatedExpenseAccountNumber);
            return ListExpenseAccountResponse.From(expenseAccount);
        }

        private async Task<TransactionalAccount> FindExpenseAccountByNumberAsync(string associatedExpenseAccountNumber)
        {
            Maybe<TransactionalAccount> maybeExpenseAccount = await _transactionalAccountRepository.GetHomeExpenseAccountByNumberAsync(associatedExpenseAccountNumber);
            if (maybeExpenseAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"TransactionalAccount with number '{associatedExpenseAccountNumber}' not found");
            }
            TransactionalAccount expenseAccount = maybeExpenseAccount.Value;
            return expenseAccount;
        }

        private async Task<SummaryExpenseCategory> FindSummaryExpenseCategoryByCodeAsync(string summaryExpenseCategoryCode)
        {
            Maybe<SummaryExpenseCategory> maybeSummaryExpenseCategory = await _summaryExpenseCategoryRepository.GetByCodeAsync(summaryExpenseCategoryCode);
            if (maybeSummaryExpenseCategory.HasNoValue)
            {
                throw new ResourceNotFoundException($"SummaryExpenseCategory with code '{summaryExpenseCategoryCode}' not found");
            }
            SummaryExpenseCategory summaryExpenseCategory = maybeSummaryExpenseCategory.Value;
            return summaryExpenseCategory;
        }
    }
}
