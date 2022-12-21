using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Core.Expenses
{
    public class CashAccountExpensePaymentMethod : ExpensePaymentMethod
    {
        public string Name { get; private set; }
        public string BankName { get; private set; }
        public AccountNumber AssociatedAccountNumber { get; private set; }

        public static CashAccountExpensePaymentMethod From(CashAccount cashAccount)
        {
            return new()
            {
                Name = cashAccount.Name,
                BankName = cashAccount.BankName,
                AssociatedAccountNumber = cashAccount.AssociatedAccountNumber
            };
        }

        public override AccountNumber GetAssociatedAccountNumber()
        {
            return AssociatedAccountNumber;
        }
    }
}