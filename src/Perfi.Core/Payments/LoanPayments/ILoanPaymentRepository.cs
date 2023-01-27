using Perfi.SharedKernel;

namespace Perfi.Core.Payments.LoanPayments
{
    public interface ILoanPaymentRepository : IRepository<LoanPayment>
    {
        LoanPayment Add(LoanPayment loanPayment);
        void Update(LoanPayment loanPayment);
    }
}
