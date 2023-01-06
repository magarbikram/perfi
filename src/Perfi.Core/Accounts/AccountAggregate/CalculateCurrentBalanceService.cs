using CSharpFunctionalExtensions;
using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Core.Accounts.AccountAggregate
{
    public class CalculateCurrentBalanceService : ICalculateCurrentBalanceService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;

        public CalculateCurrentBalanceService(
            ITransactionalAccountRepository transactionalAccountRepository,
            IAccountingTransactionRepository accountingTransactionRepository)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _accountingTransactionRepository = accountingTransactionRepository;
        }

        public async Task<Money> GetCurrentBalanceOfAccountAsync(AccountNumber accountNumber)
        {
            TransactionalAccount transactionalAccount = await FindTransactionAccountByNumber(accountNumber);
            return await GetCurrentBalanceOfAccountAsync(transactionalAccount);
        }

        private async Task<Money> GetCurrentBalanceOfAccountAsync(TransactionalAccount transactionalAccount)
        {
            IEnumerable<AccountingEntry> accountingEntries = await _accountingTransactionRepository.GetAccountingEntriesOfPeriodAsync(transactionalAccount.Number, TransactionPeriod.CurrentPeriod());
            Money sumDebitAmount = Money.Sum(accountingEntries.Where(ae => ae.DebitAmount != null).Select(ae => ae.DebitAmount!));
            Money sumCreditAmount = Money.Sum(accountingEntries.Where(ae => ae.CreditAmount != null).Select(ae => ae.CreditAmount!));
            if (transactionalAccount.BeginingBalance != null)
            {
                return transactionalAccount.BeginingBalance + sumDebitAmount - sumCreditAmount;
            }
            return sumDebitAmount - sumCreditAmount;
        }

        public async Task<Money> GetCurrentBalanceOfAccountAsync(IEnumerable<AccountNumber> accountNumbers)
        {
            IEnumerable<TransactionalAccount> transactionalAccounts = await _transactionalAccountRepository.GetByAccountNumbersAsync(accountNumbers);
            Money totalBalanceAmount = Money.UsdFrom(0);
            foreach (TransactionalAccount transactionalAccount in transactionalAccounts)
            {
                Money currentBalance = await GetCurrentBalanceOfAccountAsync(transactionalAccount);
                totalBalanceAmount += currentBalance;
            }
            return totalBalanceAmount;
        }

        private async Task<TransactionalAccount> FindTransactionAccountByNumber(AccountNumber accountNumber)
        {
            Maybe<TransactionalAccount> maybeTransactionalAccount = await _transactionalAccountRepository.GetByAccountNumberAsync(accountNumber);
            if (maybeTransactionalAccount.HasNoValue)
            {
                throw new KeyNotFoundException($"Transaction Account with number '{accountNumber}' not found");
            }
            return maybeTransactionalAccount.Value;
        }
    }
}
