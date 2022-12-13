using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.LoanAggregate
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Loan Add(Loan loan);
        Task<List<Loan>> GetAllAsync();
    }
}
