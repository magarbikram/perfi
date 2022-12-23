using CSharpFunctionalExtensions;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Accounts.AccountAggregate
{
    public interface ITransactionalAccountRepository : IRepository<TransactionalAccount>
    {
        TransactionalAccount Add(TransactionalAccount transactionalAccount);
        Task<int> CountWithSummaryAccountNumberAsync(AccountNumber jobServiceSummaryAccountNumber);
        Task<IEnumerable<TransactionalAccount>> GetAllHomeExpenseAccountsAsync();
        Task<Maybe<TransactionalAccount>> GetByAccountNumberAsync(AccountNumber accountNumber);
        Task<Maybe<TransactionalAccount>> GetHomeExpenseAccountByNumberAsync(string expenseAccountNumber);
    }
}
