using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class CloseCurrentTransactionPeriodService : ICloseCurrentTransactionPeriodService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;

        public CloseCurrentTransactionPeriodService(
            ITransactionalAccountRepository transactionalAccountRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
        }
        public async Task CloseAsync(TransactionPeriod transactionPeriod)
        {
            IEnumerable<TransactionalAccount> accounts = await _transactionalAccountRepository.GetAccountsForClosingAsync();
            foreach (TransactionalAccount account in accounts)
            {
                Money currentBalance = await _calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(account.Number);
                account.SetBeginingBalance(currentBalance);
                _transactionalAccountRepository.Update(account);
            }
            await _transactionalAccountRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}
