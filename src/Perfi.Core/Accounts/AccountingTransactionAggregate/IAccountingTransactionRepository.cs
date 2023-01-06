using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Accounts.AccountingTransactionAggregate
{
    public interface IAccountingTransactionRepository : IRepository<AccountingTransaction>
    {
        AccountingTransaction Add(AccountingTransaction accountingTransaction);
        Task<IEnumerable<AccountingEntry>> GetAccountingEntriesOfPeriodAsync(AccountNumber accountNumber, TransactionPeriod transactionPeriod);
    }
}
