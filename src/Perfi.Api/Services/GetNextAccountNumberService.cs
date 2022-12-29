using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Api.Services
{
    public class GetNextAccountNumberService : IGetNextAccountNumberService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;

        public GetNextAccountNumberService(ITransactionalAccountRepository transactionalAccountRepository)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
        }

        public async Task<AccountNumber> GetNextAsync(AccountNumber summaryAccountNumber)
        {
            int count = await _transactionalAccountRepository.CountWithSummaryAccountNumberAsync(summaryAccountNumber);
            return AccountNumber.Next(summaryAccountNumber, count);
        }
    }
}
