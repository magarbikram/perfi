using CSharpFunctionalExtensions;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.CreditCardAggregate
{
    public interface ICreditCardAccountRepository : IRepository<CashAccount>
    {
        CreditCardAccount Add(CreditCardAccount cashAccount);
        Task<IEnumerable<AccountNumber>> GetAllAccountNumbersAsync();
        Task<List<CreditCardAccount>> GetAllAsync();
        Task<Maybe<CreditCardAccount>> GetByIdAsync(int creditCardAccountId);
    }
}
