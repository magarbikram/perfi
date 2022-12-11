using CSharpFunctionalExtensions;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Accounts.AccountAggregate
{
    public interface ISummaryAccountRepository : IRepository<SummaryAccount>
    {
        Task<Maybe<SummaryAccount>> GetByNumberAsync(string accountNumber);
    }
}
