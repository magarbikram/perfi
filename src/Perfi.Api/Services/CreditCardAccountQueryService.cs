using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;

namespace Perfi.Api.Services
{
    public class CreditCardAccountQueryService : ICreditCardAccountQueryService
    {
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;

        public CreditCardAccountQueryService(
            ICreditCardAccountRepository creditCardAccountRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService)
        {
            _creditCardAccountRepository = creditCardAccountRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
        }

        public async Task<List<ListCreditCardAccountResponse>> GetAllAsync(bool withCurrentBalance)
        {
            List<CreditCardAccount> cashAccounts = await _creditCardAccountRepository.GetAllAsync();
            return await MapToResponsesAsync(cashAccounts, withCurrentBalance);
        }

        private async Task<List<ListCreditCardAccountResponse>> MapToResponsesAsync(List<CreditCardAccount> creditCardAccounts, bool withCurrentBalance)
        {
            List<ListCreditCardAccountResponse> responses = new List<ListCreditCardAccountResponse>();
            foreach (CreditCardAccount creditCardAccount in creditCardAccounts)
            {
                if (withCurrentBalance)
                {
                    Money currentBalance = await _calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(creditCardAccount.AssociatedAccountNumber);
                    responses.Add(ListCreditCardAccountResponse.From(creditCardAccount, currentBalance));
                }
                else
                {
                    responses.Add(ListCreditCardAccountResponse.From(creditCardAccount));
                }

            }
            return responses;
        }
    }
}
