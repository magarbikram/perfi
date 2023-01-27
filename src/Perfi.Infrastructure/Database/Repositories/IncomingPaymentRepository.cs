using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.Core.Payments.IncomingPayments;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class IncomingPaymentRepository : IIncomingPaymentRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public IncomingPaymentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IncomingPayment Add(IncomingPayment incomingPayment)
        {
            return _appDbContext.IncomingPayments.Add(incomingPayment).Entity;
        }

        public async Task<Money> GetTotalAmountAsync(TransactionPeriod transactionPeriod)
        {
            IEnumerable<Money> incomingAmounts = await _appDbContext.IncomingPayments
                                .Where(ip => ip.TransactionPeriod == transactionPeriod)
                                .Select(ip => ip.Amount).AsNoTracking().ToListAsync();
            return Money.Sum(incomingAmounts);
        }
    }
}
