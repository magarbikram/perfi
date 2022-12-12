using Perfi.Api.Responses;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Api.Services
{
    public class CashAccountQueryService : ICashAccountQueryService
    {
        private readonly ICashAccountRepository _cashAccountRepository;

        public CashAccountQueryService(ICashAccountRepository cashAccountRepository)
        {
            _cashAccountRepository = cashAccountRepository;
        }

        public async Task<List<ListCashAccountResponse>> GetAllAsync()
        {
            List<CashAccount> cashAccounts = await _cashAccountRepository.GetAllAsync();
            return MapToResponses(cashAccounts);
        }

        private static List<ListCashAccountResponse> MapToResponses(List<CashAccount> cashAccounts)
        {
            List<ListCashAccountResponse> responses = new List<ListCashAccountResponse>();
            foreach (CashAccount cashAccount in cashAccounts)
            {
                responses.Add(ListCashAccountResponse.From(cashAccount));
            }
            return responses;
        }
    }
}
