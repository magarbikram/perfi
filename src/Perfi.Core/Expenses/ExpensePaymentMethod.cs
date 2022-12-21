using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Expenses
{
    public abstract class ExpensePaymentMethod : BaseEntity
    {
        public abstract AccountNumber GetAssociatedAccountNumber();
    }
}