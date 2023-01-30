using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate.QueryModels;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.AccountingTransactionAggregate
{
    public interface IAccountingTransactionRepository : IRepository<AccountingTransaction>
    {
        AccountingTransaction Add(AccountingTransaction accountingTransaction);
        Task<IEnumerable<AccountingEntry>> GetAccountingEntriesOfPeriodAsync(AccountNumber accountNumber, TransactionPeriod transactionPeriod);
        Task<IEnumerable<Transaction>> GetAccountingTransactionsOfPeriodAsync(AccountNumber accountNumber, TransactionPeriod transactionPeriod);
    }
}
