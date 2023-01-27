using Perfi.Core.Payments.LoanPayments;

namespace Perfi.Api.Responses
{
    public class NewLoanPaymentAddedResponse
    {
        public int LoanPaymentId { get; set; }
        public static NewLoanPaymentAddedResponse From(LoanPayment loanPayment)
        {
            return new NewLoanPaymentAddedResponse
            {
                LoanPaymentId = loanPayment.Id
            };
        }
    }
}
