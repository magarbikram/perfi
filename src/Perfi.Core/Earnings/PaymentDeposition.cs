using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Earnings
{
    public abstract class PaymentDeposition : BaseEntity
    {
        public abstract AccountNumber GetAssociatedAccountNumber();
    }
}