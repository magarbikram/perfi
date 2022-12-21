using Perfi.Api.Commands;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class ExpensePaymentMethodResponse
    {
        public PaymentMethodType PaymentMethodType { get; private set; }
        public CreditCardPaymentMethodResponse CreditCardPaymentMethod { get; private set; }
        public CashAccountPaymentMethodResponse CashAccountPaymentMethod { get; private set; }

        public static ExpensePaymentMethodResponse From(ExpensePaymentMethod paymentMethod)
        {
            if (paymentMethod is CreditCardExpensePaymentMethod)
            {
                return FromCreditCard((CreditCardExpensePaymentMethod)paymentMethod);
            }
            return FromCashAccount((CashAccountExpensePaymentMethod)paymentMethod);
        }

        private static ExpensePaymentMethodResponse FromCreditCard(CreditCardExpensePaymentMethod creditCardExpensePaymentMethod)
        {
            return new ExpensePaymentMethodResponse
            {
                PaymentMethodType = PaymentMethodType.CreditCard,
                CreditCardPaymentMethod = CreditCardPaymentMethodResponse.From(creditCardExpensePaymentMethod)
            };
        }

        private static ExpensePaymentMethodResponse FromCashAccount(CashAccountExpensePaymentMethod cashAccountExpensePaymentMethod)
        {
            return new ExpensePaymentMethodResponse
            {
                PaymentMethodType = PaymentMethodType.CashAccount,
                CashAccountPaymentMethod = CashAccountPaymentMethodResponse.From(cashAccountExpensePaymentMethod)
            };
        }
    }
}
