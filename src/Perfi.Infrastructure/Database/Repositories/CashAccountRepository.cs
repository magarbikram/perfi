using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class CashAccountRepository : ICashAccountRepository
    {
        private readonly AppDbContext _appDbContext;

        public CashAccountRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IUnitOfWork UnitOfWork => _appDbContext;

        public CashAccount Add(CashAccount cashAccount)
        {
            return _appDbContext.Add(cashAccount).Entity;
        }
    }
}
