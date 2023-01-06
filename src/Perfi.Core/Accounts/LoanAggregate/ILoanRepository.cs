using CSharpFunctionalExtensions;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.LoanAggregate
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Loan Add(Loan loan);
        Task<IEnumerable<AccountNumber>> GetAllAccountNumbersAsync();
        Task<List<Loan>> GetAllAsync();
        Task<Maybe<Loan>> GetByIdAsync(int mortgageLoanId);
    }
}
