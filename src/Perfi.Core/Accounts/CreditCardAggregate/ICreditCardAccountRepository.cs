﻿using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.CreditCardAggregate
{
    public interface ICreditCardAccountRepository : IRepository<CashAccount>
    {
        CreditCardAccount Add(CreditCardAccount cashAccount);
        Task<List<CreditCardAccount>> GetAllAsync();
    }
}
