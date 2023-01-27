using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Core.Payments.IncomingPayments
{
    public interface IIncomingPaymentRepository : IRepository<IncomingPayment>
    {
        IncomingPayment Add(IncomingPayment incomingPayment);
        Task<Money> GetTotalAmountAsync(TransactionPeriod transactionPeriod);
    }
}
