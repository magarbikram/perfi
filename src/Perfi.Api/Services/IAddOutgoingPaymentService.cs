using Perfi.Core.Accounting;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public interface IAddOutgoingPaymentService
    {
        void AddOutGoingPaymentFor(Expense expense, Money amount);
    }
}