using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Core.Payments.OutgoingPayments
{
    public interface IOutgoingPaymentRepository : IRepository<OutgoingPayment>
    {
        OutgoingPayment Add(OutgoingPayment outgoingPayment);
        Task<Money> GetTotalAmountAsync(TransactionPeriod transactionPeriod);
    }
}
