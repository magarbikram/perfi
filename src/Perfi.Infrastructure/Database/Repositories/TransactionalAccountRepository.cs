using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class TransactionalAccountRepository : ITransactionalAccountRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public TransactionalAccountRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public TransactionalAccount Add(TransactionalAccount transactionalAccount)
        {
            return _appDbContext.TransactionalAccounts.Add(transactionalAccount).Entity;
        }

        public async Task<IEnumerable<TransactionalAccount>> GetAllHomeExpenseAccountsAsync()
        {
            return await _appDbContext.TransactionalAccounts
                                     .Where(ta => ta.ParentAccountNumber == GetSummaryHomeExpenseAccountNumber())
                                     .ToListAsync();
        }

        private static AccountNumber GetSummaryHomeExpenseAccountNumber()
        {
            return AccountNumber.From(SummaryAccount.DefaultAccountNumbers.HomeExpensesAccount);
        }

        public async Task<Maybe<TransactionalAccount>> GetHomeExpenseAccountByNumberAsync(string expenseAccountNumber)
        {
            TransactionalAccount? transactionalAccount = await _appDbContext.TransactionalAccounts.FirstOrDefaultAsync(ta => ta.ParentAccountNumber == GetSummaryHomeExpenseAccountNumber() &&
                                                                                                        ta.Number == AccountNumber.From(expenseAccountNumber));
            if (transactionalAccount == null)
            {
                return Maybe<TransactionalAccount>.None;
            }
            return transactionalAccount;
        }

        public async Task<Maybe<TransactionalAccount>> GetByAccountNumberAsync(AccountNumber accountNumber)
        {
            TransactionalAccount? transactionalAccount = await _appDbContext.TransactionalAccounts.FirstOrDefaultAsync(ta =>
                                                                                                        ta.Number == accountNumber);
            if (transactionalAccount == null)
            {
                return Maybe<TransactionalAccount>.None;
            }
            return transactionalAccount;
        }

        public async Task<int> CountWithSummaryAccountNumberAsync(AccountNumber summaryAccountNumber)
        {
            return await _appDbContext.TransactionalAccounts.CountAsync(ta => ta.ParentAccountNumber == summaryAccountNumber);
        }

        public async Task<IEnumerable<TransactionalAccount>> GetByAccountNumbersAsync(IEnumerable<AccountNumber> accountNumbers)
        {
            return await _appDbContext.TransactionalAccounts.Where(ta => accountNumbers.Contains(ta.Number)).ToListAsync();
        }

        public void Update(TransactionalAccount account)
        {
            _appDbContext.TransactionalAccounts.Update(account);
        }

        public async Task<IEnumerable<TransactionalAccount>> GetAccountsForClosingAsync()
        {
            return await _appDbContext.TransactionalAccounts
                                        .Where(ta => ta.AccountCategory == AccountCategory.Assets ||
                                                     ta.AccountCategory == AccountCategory.Liabilities)
                                        .ToListAsync();
        }
    }
}
