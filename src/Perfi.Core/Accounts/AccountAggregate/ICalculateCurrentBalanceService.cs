using Perfi.Core.Accounting;
using Perfi.Core.Accounts.CashAccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Accounts.AccountAggregate
{
    public interface ICalculateCurrentBalanceService
    {
        Task<Money> GetCurrentBalanceOfAccountAsync(AccountNumber accountNumber);
        Task<Money> GetCurrentBalanceOfAccountAsync(IEnumerable<AccountNumber> cashAccountNumbers);
    }
}
