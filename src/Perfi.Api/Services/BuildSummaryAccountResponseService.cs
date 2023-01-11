using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Services
{
    public class BuildSummaryAccountResponseService : IBuildSummaryAccountResponseService
    {
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ISplitPartnerRepository _splitPartnerRepository;

        public BuildSummaryAccountResponseService(
            ICashAccountRepository cashAccountRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService,
            ICreditCardAccountRepository creditCardAccountRepository,
            ILoanRepository loanRepository,
            ISplitPartnerRepository splitPartnerRepository)
        {
            _cashAccountRepository = cashAccountRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
            _creditCardAccountRepository = creditCardAccountRepository;
            _loanRepository = loanRepository;
            _splitPartnerRepository = splitPartnerRepository;
        }
        public async Task<AccountSummaryResponse> Build()
        {
            AccountSummaryResponse accountSummaryResponseaccountSummaryResponse = new AccountSummaryResponse
            {
                CashAccountsBalance = await CalculateCashAccountBalanceAsync(),
                CreditCardAccountsBalance = await CalculateCreditCardAccountsBalanceAsync(),
                LoansBalance = await CalculateLoanAccountsBalanceAsync(),
                SplitPartnersBalance = await CalculateSplitPartnersBalanceAsync()
            };

            return accountSummaryResponseaccountSummaryResponse;
        }

        private async Task<MoneyResponse> CalculateSplitPartnersBalanceAsync()
        {
            IEnumerable<AccountNumber> loanAccountNumbers = await _splitPartnerRepository.GetAllAccountNumbersAsync();
            Money money = await _calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(loanAccountNumbers);
            return MoneyResponse.From(money);
        }

        private async Task<MoneyResponse> CalculateLoanAccountsBalanceAsync()
        {
            IEnumerable<AccountNumber> loanAccountNumbers = await _loanRepository.GetAllAccountNumbersAsync();
            Money money = await _calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(loanAccountNumbers);
            return MoneyResponse.From(money);
        }

        private async Task<MoneyResponse> CalculateCreditCardAccountsBalanceAsync()
        {
            IEnumerable<AccountNumber> creditCardAccountNumbers = await _creditCardAccountRepository.GetAllAccountNumbersAsync();
            Money money = await _calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(creditCardAccountNumbers);
            return MoneyResponse.From(money);
        }

        private async Task<MoneyResponse> CalculateCashAccountBalanceAsync()
        {
            IEnumerable<AccountNumber> cashAccountNumbers = await _cashAccountRepository.GetAllAccountNumbersAsync();
            Money money = await _calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(cashAccountNumbers);
            return MoneyResponse.From(money);
        }
    }
}
