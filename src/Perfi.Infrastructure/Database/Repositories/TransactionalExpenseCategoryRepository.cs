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
    }
}
