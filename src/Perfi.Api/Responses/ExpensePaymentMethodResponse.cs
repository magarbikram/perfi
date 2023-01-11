using Perfi.Api.Commands;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class ExpensePaymentMethodResponse
    {
        public PaymentMethodType PaymentMethodType { get; private set; }
        public CreditCardPaymentMethodResponse CreditCardPaymentMethod { get; private set; }
        public CashAccountPaymentMethodResponse CashAccountPaymentMethod { get; private set; }
        public SplitPartnerPaymentMethodResponse SplitPartnerPaymentMethod { get; private set; }

        public static ExpensePaymentMethodResponse From(ExpensePaymentMethod paymentMethod)
        {
            if (paymentMethod is CreditCardExpensePaymentMethod)
            {
                return FromCreditCard((CreditCardExpensePaymentMethod)paymentMethod);
            }
            else if (paymentMethod is CashAccountExpensePaymentMethod)
            {
                return FromCashAccount((CashAccountExpensePaymentMethod)paymentMethod);
            }
            return FromSplitPartner((SplitPartnerExpensePaymentMethod)paymentMethod);
        }

        private static ExpensePaymentMethodResponse FromSplitPartner(SplitPartnerExpensePaymentMethod paymentMethod)
        {
            return new ExpensePaymentMethodResponse
            {
                PaymentMethodType = PaymentMethodType.SplitPartner,
                SplitPartnerPaymentMethod = SplitPartnerPaymentMethodResponse.From(paymentMethod)
            };
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
