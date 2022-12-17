using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Api.Services
{
    public class ExpenseAccountQueryService : IExpenseAccountQueryService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;

        public ExpenseAccountQueryService(ITransactionalAccountRepository transactionalAccountRepository)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
        }

        public async Task<IEnumerable<ListExpenseAccountResponse>> GetAllAsync()
        {
            IEnumerable<TransactionalAccount> homeExpenseAccounts = await _transactionalAccountRepository.GetAllHomeExpenseAccountsAsync();
            return MapToResponses(homeExpenseAccounts);
        }

        private static IEnumerable<ListExpenseAccountResponse> MapToResponses(IEnumerable<TransactionalAccount> homeExpenseAccounts)
        {
            List<ListExpenseAccountResponse> responses = new List<ListExpenseAccountResponse>();
            foreach (TransactionalAccount transactionalAccount in homeExpenseAccounts)
            {
                responses.Add(ListExpenseAccountResponse.From(transactionalAccount));
            }
            return responses;
        }
    }
}
