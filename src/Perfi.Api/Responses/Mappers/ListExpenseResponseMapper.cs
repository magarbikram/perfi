using Perfi.Api.Models;
using Perfi.Core.Expenses;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Responses.Mappers
{
    public class ListExpenseResponseMapper
    {
        private readonly ITransactionalExpenseCategoryRepository _transactionalExpenseCategoryRepository;
        private readonly ISplitPartnerRepository _splitPartnerRepository;

        public ListExpenseResponseMapper(
            ITransactionalExpenseCategoryRepository transactionalExpenseCategoryRepository,
            ISplitPartnerRepository splitPartnerRepository)
        {
            _transactionalExpenseCategoryRepository = transactionalExpenseCategoryRepository;
            _splitPartnerRepository = splitPartnerRepository;
        }

        public async Task<List<ListExpenseResponse>> MapAsync(IEnumerable<Expense> expenses)
        {
            List<ExpenseWithChildEntity> expenseWithTransactionCategoryDetails = await PrepareExpenseWithTransactionCategoryDetails(expenses);
            return ListExpenseResponse.From(expenseWithTransactionCategoryDetails);
        }

        private async Task<List<ExpenseWithChildEntity>> PrepareExpenseWithTransactionCategoryDetails(IEnumerable<Expense> expenses)
        {
            ISet<ExpenseCategoryCode> expenseCategoryCodes = expenses.Select(exp => exp.ExpenseCategoryCode).Distinct().ToHashSet();
            IEnumerable<TransactionalExpenseCategory> transactionalExpenseCategories = await _transactionalExpenseCategoryRepository.GetByCodesAsync(expenseCategoryCodes);
            IEnumerable<SplitPartner> splitPartners = await _splitPartnerRepository.GetByIdsAsync(expenses.Where(exp => exp.IsSplit()).Select(exp => exp.SplitPayment!.SplitPartnerId).Distinct());
            IDictionary<int, SplitPartner> splitPartnerDictionary = splitPartners.ToDictionary(sp => sp.Id);
            IDictionary<ExpenseCategoryCode, TransactionalExpenseCategory> indexedTransactionalExpenseCategories =
                transactionalExpenseCategories.ToDictionary(tec => tec.Code);
            List<ExpenseWithChildEntity> expenseWithTransactionCategoryDetails = new();
            foreach (Expense expense in expenses)
            {
                TransactionalExpenseCategory transactionalExpenseCategory = indexedTransactionalExpenseCategories[expense.ExpenseCategoryCode];
                ExpenseWithChildEntity expenseWithChildEntity = ExpenseWithChildEntity.From(expense, transactionalExpenseCategory);
                if (expense.IsSplit())
                {
                    SplitPartner splitPartner = splitPartnerDictionary[expense.SplitPayment!.SplitPartnerId];
                    expenseWithChildEntity.SetSplitPartner(splitPartner);
                }
                expenseWithTransactionCategoryDetails.Add(expenseWithChildEntity);
            }

            return expenseWithTransactionCategoryDetails;
        }
    }
}
