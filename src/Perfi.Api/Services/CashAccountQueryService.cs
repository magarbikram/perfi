using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Api.Services
{
    public class CashAccountQueryService : ICashAccountQueryService
    {
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;

        public CashAccountQueryService(
            ICashAccountRepository cashAccountRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService)
        {
            _cashAccountRepository = cashAccountRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
        }

        public async Task<List<ListCashAccountResponse>> GetAllAsync(bool withCurrentBalance)
        {
            List<CashAccount> cashAccounts = await _cashAccountRepository.GetAllAsync();
            return await MapToResponses(cashAccounts, withCurrentBalance);
        }

        private async Task<List<ListCashAccountResponse>> MapToResponses(List<CashAccount> cashAccounts, bool withCurrentBalance)
        {
            List<ListCashAccountResponse> responses = new List<ListCashAccountResponse>();
            foreach (CashAccount cashAccount in cashAccounts)
            {
                if (withCurrentBalance)
                {
                    Money currentBalance = await _calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(cashAccount.AssociatedAccountNumber);
                    responses.Add(ListCashAccountResponse.From(cashAccount, currentBalance));
                }
                else
                {
                    responses.Add(ListCashAccountResponse.From(cashAccount));
                }

            }
            return responses;
        }
    }
}
