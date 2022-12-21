using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class TransactionalExpenseCategoryRepository : ITransactionalExpenseCategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;
        public TransactionalExpenseCategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public TransactionalExpenseCategory Add(TransactionalExpenseCategory category)
        {
            return _appDbContext.Add(category).Entity;
        }

        public async Task<Maybe<TransactionalExpenseCategory>> GetByCodeAsync(ExpenseCategoryCode expenseCategoryCode)
        {
            TransactionalExpenseCategory? transactionalExpenseCategory = await _appDbContext.TransactionalExpenseCategories.FirstOrDefaultAsync(cca => cca.Code == expenseCategoryCode);
            if (transactionalExpenseCategory == null)
            {
                return Maybe<TransactionalExpenseCategory>.None;
            }
            return transactionalExpenseCategory;
        }
    }
}
