using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class SummaryAccountRepository : ISummaryAccountRepository
    {
        private readonly AppDbContext _appDbContext;

        public SummaryAccountRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IUnitOfWork UnitOfWork => _appDbContext;

        public async Task<Maybe<SummaryAccount>> GetByNumberAsync(string accountNumber)
        {
            SummaryAccount? summaryAccount = await _appDbContext.SummaryAccounts.FirstOrDefaultAsync(account => account.Number == AccountNumber.From(accountNumber));
            if (summaryAccount == null)
            {
                return Maybe<SummaryAccount>.None;
            }
            return summaryAccount;
        }
    }
}
