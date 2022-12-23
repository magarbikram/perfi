using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Api.Services
{
    public interface IGetNextAccountNumberService
    {
        Task<AccountNumber> GetNext(AccountNumber summaryAccountNumber);
    }
}