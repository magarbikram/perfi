using CSharpFunctionalExtensions;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate.QueryModels;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class CreditCardAccountQueryService : ICreditCardAccountQueryService
    {
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;

        public CreditCardAccountQueryService(
            ICreditCardAccountRepository creditCardAccountRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService,
            IAccountingTransactionRepository accountingTransactionRepository)
        {
            _creditCardAccountRepository = creditCardAccountRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
            _accountingTransactionRepository = accountingTransactionRepository;
        }

        public async Task<List<ListCreditCardAccountResponse>> GetAllAsync(bool withCurrentBalance)
        {
            List<CreditCardAccount> cashAccounts = await _creditCardAccountRepository.GetAllAsync();
            return await MapToResponsesAsync(cashAccounts, withCurrentBalance);
        }

        public async Task<List<TransactionResponse>> GetAllTransactionsAsync(int creditCardId, TransactionPeriod transactionPeriod)
        {
            Maybe<CreditCardAccount> maybeCreditCardAccount = await _creditCardAccountRepository.GetByIdAsync(creditCardId);
            if (maybeCreditCardAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"CreditCardAccount with id '{creditCardId}' not found");
            }
            CreditCardAccount creditCardAccount = maybeCreditCardAccount.Value;
            IEnumerable<Transaction> transactions = await _accountingTransactionRepository.GetAccountingTransactionsOfPeriodAsync(creditCardAccount.AssociatedAccountNumber, transactionPeriod);
            return TransactionResponse.From(transactions);

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
