﻿using CSharpFunctionalExtensions;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Accounts.CashAccountAggregate
{
    public interface ICashAccountRepository : IRepository<CashAccount>
    {
        CashAccount Add(CashAccount cashAccount);
        Task<IEnumerable<AccountNumber>> GetAllAccountNumbersAsync();
        Task<List<CashAccount>> GetAllAsync();
        Task<Maybe<CashAccount>> GetByIdAsync(int cashAccountId);
    }
}
