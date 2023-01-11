using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Services
{
    public class AddSplitPartnerService : IAddSplitPartnerService
    {
        private readonly ISplitPartnerRepository _splitPartnerRepository;
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly IGetNextAccountNumberService _getNextAccountNumberService;

        public AddSplitPartnerService(
            ISplitPartnerRepository splitPartnerRepository,
            ITransactionalAccountRepository transactionalAccountRepository,
            IGetNextAccountNumberService getNextAccountNumberService)
        {
            _splitPartnerRepository = splitPartnerRepository;
            _transactionalAccountRepository = transactionalAccountRepository;
            _getNextAccountNumberService = getNextAccountNumberService;
        }

        public async Task<NewSplitPartnerAddedResponse> AddAsync(AddNewSplitParnerCommand addNewSplitParnerCommand)
        {
            SplitPartner splitPartner = AddSplitPartner(addNewSplitParnerCommand);
            TransactionalAccount splitPartnerAccountReceivable = await AddAccountsReceivable(addNewSplitParnerCommand);
            splitPartner.SetAccountsPayable(splitPartnerAccountReceivable);
            await _splitPartnerRepository.UnitOfWork.SaveChangesAsync();
            return NewSplitPartnerAddedResponse.From(splitPartner);
        }

        private async Task<TransactionalAccount> AddAccountsReceivable(AddNewSplitParnerCommand addNewSplitParnerCommand)
        {
            AccountNumber receivableSummaryAccountNumber = SummaryAccount.DefaultAccountNumbers.GetReceivableAccountNumber();
            AccountNumber accountNumber = await _getNextAccountNumberService.GetNextAsync(receivableSummaryAccountNumber);
            TransactionalAccount accountReceivable = TransactionalAccount.NewAssetAccount(accountNumber, addNewSplitParnerCommand.Name, receivableSummaryAccountNumber);
            accountReceivable = _transactionalAccountRepository.Add(accountReceivable);
            return accountReceivable;
        }

        private SplitPartner AddSplitPartner(AddNewSplitParnerCommand addNewSplitParnerCommand)
        {
            SplitPartner splitPartner = SplitPartner.From(addNewSplitParnerCommand.Name);
            splitPartner = _splitPartnerRepository.Add(splitPartner);
            return splitPartner;
        }
    }
}
