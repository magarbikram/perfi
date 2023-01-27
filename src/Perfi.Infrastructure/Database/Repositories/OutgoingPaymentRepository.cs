using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.Core.Payments.OutgoingPayments;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class OutgoingPaymentRepository : IOutgoingPaymentRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public OutgoingPaymentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public OutgoingPayment Add(OutgoingPayment outgoingPayment)
        {
            return _appDbContext.OutgoingPayments.Add(outgoingPayment).Entity;
        }

        public async Task<Money> GetTotalAmountAsync(TransactionPeriod transactionPeriod)
        {
            IEnumerable<Money> outgoingAmounts = await _appDbContext.OutgoingPayments
                                .Where(ip => ip.TransactionPeriod == transactionPeriod)
                                .Select(ip => ip.Amount).AsNoTracking().ToListAsync();
            return Money.Sum(outgoingAmounts);
        }
    }
}
