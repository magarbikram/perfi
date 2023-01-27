using CSharpFunctionalExtensions;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.SplitPartners
{
    public interface ISplitPartnerRepository : IRepository<SplitPartner>
    {
        SplitPartner Add(SplitPartner splitPartner);
        Task<Maybe<SplitPartner>> GetByIdAsync(int id);
        Task<IEnumerable<SplitPartner>> GetAllAsync();
        Task<IEnumerable<AccountNumber>> GetAllAccountNumbersAsync();
        Task<IEnumerable<SplitPartner>> GetByIdsAsync(IEnumerable<int> enumerable);
    }
}
