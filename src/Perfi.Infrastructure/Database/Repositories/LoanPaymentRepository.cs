using Perfi.Core.Payments.LoanPayments;
using Perfi.SharedKernel;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class LoanPaymentRepository : ILoanPaymentRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public LoanPaymentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public LoanPayment Add(LoanPayment loanPayment)
        {
            return _appDbContext.LoanPayments.Add(loanPayment).Entity;
        }

        public void Update(LoanPayment loanPayment)
        {
            _appDbContext.LoanPayments.Update(loanPayment);
        }
    }
}
