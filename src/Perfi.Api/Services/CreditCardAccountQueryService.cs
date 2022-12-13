using Perfi.Api.Responses;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;

namespace Perfi.Api.Services
{
    public class CreditCardAccountQueryService : ICreditCardAccountQueryService
    {
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;

        public CreditCardAccountQueryService(ICreditCardAccountRepository creditCardAccountRepository)
        {
            _creditCardAccountRepository = creditCardAccountRepository;
        }

        public async Task<List<ListCreditCardAccountResponse>> GetAllAsync()
        {
            List<CreditCardAccount> cashAccounts = await _creditCardAccountRepository.GetAllAsync();
            return MapToResponses(cashAccounts);
        }

        private static List<ListCreditCardAccountResponse> MapToResponses(List<CreditCardAccount> creditCardAccounts)
        {
            List<ListCreditCardAccountResponse> responses = new List<ListCreditCardAccountResponse>();
            foreach (CreditCardAccount creditCardAccount in creditCardAccounts)
            {
                responses.Add(ListCreditCardAccountResponse.From(creditCardAccount));
            }
            return responses;
        }
    }
}
