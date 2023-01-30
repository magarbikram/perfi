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
    public class CashAccountQueryService : ICashAccountQueryService
    {
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;

        public CashAccountQueryService(
            ICashAccountRepository cashAccountRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService,
            IAccountingTransactionRepository accountingTransactionRepository)
        {
            _cashAccountRepository = cashAccountRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
            _accountingTransactionRepository = accountingTransactionRepository;
        }

        public async Task<List<ListCashAccountResponse>> GetAllAsync(bool withCurrentBalance)
        {
            List<CashAccount> cashAccounts = await _cashAccountRepository.GetAllAsync();
            return await MapToResponses(cashAccounts, withCurrentBalance);
        }
        public async Task<List<TransactionResponse>> GetAllTransactionsAsync(int cashAccountId, TransactionPeriod transactionPeriod)
        {
            Maybe<CashAccount> maybeCashAccount = await _cashAccountRepository.GetByIdAsync(cashAccountId);
            if (maybeCashAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"CashAccount with id '{cashAccountId}' not found");
            }
            CashAccount cashAccount = maybeCashAccount.Value;
            IEnumerable<Transaction> transactions = await _accountingTransactionRepository.GetAccountingTransactionsOfPeriodAsync(cashAccount.AssociatedAccountNumber, transactionPeriod);
            return TransactionResponse.From(transactions);

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
