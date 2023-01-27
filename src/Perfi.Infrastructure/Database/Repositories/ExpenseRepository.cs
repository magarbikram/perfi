using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.Core.Expenses.QueryModels;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            return await _appDbContext.Expenses.Include(exp => exp.PaymentMethod).Where(exp => exp.TransactionPeriod == transactionPeriod).OrderByDescending(exp => exp.TransactionDate).ToListAsync();
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

        public void Update(Expense expense)
        {
            _appDbContext.Expenses.Update(expense);
        }

        public async Task<IEnumerable<ExpenseByCategory>> GetExpenseByCategoryAsync(TransactionPeriod transactionPeriod)
        {
            var query = from expense in _appDbContext.Expenses.Where(ex => ex.TransactionPeriod == transactionPeriod)
                        join transactionalExpenseCategory in _appDbContext.TransactionalExpenseCategories on expense.ExpenseCategoryCode equals transactionalExpenseCategory.Code
                        join summaryExpenseCategory in _appDbContext.SummaryExpenseCategories on transactionalExpenseCategory.SummaryExpenseCategoryCode equals summaryExpenseCategory.Code
                        select new ExpenseByCategory
                        {
                            ExpenseAmount = expense.Amount,
                            ExpenseCategoryCode = transactionalExpenseCategory.Code.Value,
                            ExpenseCategoryName = transactionalExpenseCategory.Name,
                            ParentExpenseCategoryCode = summaryExpenseCategory.Code.Value,
                            ParentExpenseCategoryName = summaryExpenseCategory.Name,
                        };
            var flatExpensesByCategory = await query.AsNoTracking().ToListAsync();
            return flatExpensesByCategory
                            .GroupBy(x => new { x.ExpenseCategoryCode })
                            .Select(g =>
                            {
                                ExpenseByCategory firstElement = g.First();
                                return new ExpenseByCategory
                                {
                                    ExpenseCategoryCode = firstElement.ExpenseCategoryCode,
                                    ExpenseCategoryName = firstElement.ExpenseCategoryName,
                                    ExpenseAmount = Money.Sum(g.Select(x => x.ExpenseAmount)),
                                    ParentExpenseCategoryCode = firstElement.ParentExpenseCategoryCode,
                                    ParentExpenseCategoryName = firstElement.ParentExpenseCategoryName
                                };
                            });
        }
    }
}
