using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;
        public ExpenseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Expense Add(Expense expense)
        {
            return _appDbContext.Expenses.Add(expense).Entity;
        }

        public async Task<IEnumerable<Expense>> GetAllForTransactionPeriodAsync(TransactionPeriod transactionPeriod)
        {
            return await _appDbContext.Expenses.Include(exp => exp.PaymentMethod).Where(exp => exp.TransactionPeriod == transactionPeriod).ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetTop10ExpensesForTransactionPeriodAsync(TransactionPeriod transactionPeriod)
        {
            return await _appDbContext.Expenses
                                      .Include(exp => exp.PaymentMethod)
                                      .Where(exp => exp.TransactionPeriod == transactionPeriod)
                                      .OrderByDescending(exp => exp.TransactionDate)
                                      .Take(10)
                                      .ToListAsync();
        }

        public async Task<Money> GetTotalExpenseAmountForPeriodAsync(TransactionPeriod currentTransactionPeriod)
        {
            decimal totalExpenseAmountValue = await _appDbContext.Expenses
                                                        .Where(exp => exp.TransactionPeriod == currentTransactionPeriod)
                                                        .Select(inc => inc.Amount)
                                                        .SumAsync(exa => exa.Value);
            return Money.UsdFrom(totalExpenseAmountValue);
        }
    }
}
