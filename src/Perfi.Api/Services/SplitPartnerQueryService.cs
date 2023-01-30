using CSharpFunctionalExtensions;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate.QueryModels;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Core.Expenses;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Services
{
    public class SplitPartnerQueryService : ISplitPartnerQueryService
    {
        private readonly ISplitPartnerRepository _splitPartnerRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;

        public SplitPartnerQueryService(
            ISplitPartnerRepository splitPartnerRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService,
            IAccountingTransactionRepository accountingTransactionRepository)
        {
            _splitPartnerRepository = splitPartnerRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
            _accountingTransactionRepository = accountingTransactionRepository;
        }
        public async Task<IEnumerable<ListSplitPartnerResponse>> GetAllAsync()
        {
            IEnumerable<SplitPartner> splitPartners = await _splitPartnerRepository.GetAllAsync();
            return ListSplitPartnerResponse.From(splitPartners);
        }

        public async Task<IEnumerable<ListSplitPartnerWithCurrentBalanceResponse>> GetAllWithCurrentBalanceAsync()
        {
            IEnumerable<SplitPartner> splitPartners = await _splitPartnerRepository.GetAllAsync();
            return await ListSplitPartnerWithCurrentBalanceResponse.FromAsync(splitPartners, _calculateCurrentBalanceService);
        }

        public async Task<List<TransactionResponse>> GetAllTransactionsAsync(int creditCardId, TransactionPeriod transactionPeriod)
        {
            Maybe<SplitPartner> maybeSplitPartner = await _splitPartnerRepository.GetByIdAsync(creditCardId);
            if (maybeSplitPartner.HasNoValue)
            {
                throw new ResourceNotFoundException($"SplitPartner with id '{creditCardId}' not found");
            }
            SplitPartner splitPartner = maybeSplitPartner.Value;
            IEnumerable<Transaction> transactions = await _accountingTransactionRepository.GetAccountingTransactionsOfPeriodAsync(splitPartner.ReceivableAccountNumber, transactionPeriod);
            return TransactionResponse.From(transactions);

        }
    }
}
