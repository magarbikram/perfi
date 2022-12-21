using Ardalis.GuardClauses;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class CashAccountPaymentMethodResponse
    {
        public string Name { get; set; }
        public string BankName { get; set; }
        public static CashAccountPaymentMethodResponse From(CashAccount cashAccount)
        {
            Guard.Against.Null(cashAccount, nameof(cashAccount));
            return new CashAccountPaymentMethodResponse { Name = cashAccount.Name, BankName = cashAccount.BankName };
        }

        public static CashAccountPaymentMethodResponse From(CashAccountExpensePaymentMethod cashAccountExpensePaymentMethod)
        {
            Guard.Against.Null(cashAccountExpensePaymentMethod, nameof(cashAccountExpensePaymentMethod));
            return new CashAccountPaymentMethodResponse { Name = cashAccountExpensePaymentMethod.Name, BankName = cashAccountExpensePaymentMethod.BankName };
        }
    }
}
