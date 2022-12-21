using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Expenses
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Expense Add(Expense expense);
        Task<IEnumerable<Expense>> GetAllForTransactionPeriodAsync(TransactionPeriod transactionPeriod);
    }
}
