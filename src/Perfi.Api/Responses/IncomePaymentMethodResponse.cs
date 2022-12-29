using Perfi.Api.Commands;
using Perfi.Core.Earnings;

namespace Perfi.Api.Responses
{
    public class IncomePaymentMethodResponse
    {
        public PaymentMethodType PaymentMethodType { get; private set; }
        public CashAccountPaymentMethodResponse CashAccountPaymentMethod { get; private set; }

        public static IncomePaymentMethodResponse From(IncomeDocument incomeDocument)
        {
            return new IncomePaymentMethodResponse
            {
                PaymentMethodType = PaymentMethodType.CashAccount,
                CashAccountPaymentMethod = CashAccountPaymentMethodResponse.From((PaymenDepositionToCashAccount)incomeDocument.PaymentDeposition)
            };
        }
    }
}
