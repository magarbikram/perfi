using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Earnings.IncomeSources;

namespace Perfi.Api.Services
{
    public class AddNewIncomeSourceService : IAddNewIncomeSourceService
    {
        private readonly IIncomeSourceRepository _incomeSourceRepository;
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly IGetNextAccountNumberService _getNextAccountNumberService;

        public AddNewIncomeSourceService(
            IIncomeSourceRepository incomeSourceRepository,
            ITransactionalAccountRepository transactionalAccountRepository,
            IGetNextAccountNumberService getNextAccountNumberService)
        {
            _incomeSourceRepository = incomeSourceRepository;
            _transactionalAccountRepository = transactionalAccountRepository;
            _getNextAccountNumberService = getNextAccountNumberService;
        }

        public async Task<NewIncomeSourceAddedResponse> AddAsync(AddNewIncomeSourceCommand addNewIncomeSourceCommand)
        {
            TransactionalAccount transactionalAccount = await AddAssociatedTransactionalAccountAsync(addNewIncomeSourceCommand);
            IncomeSource incomeSource = IncomeSource.From(name: addNewIncomeSourceCommand.Name, type: addNewIncomeSourceCommand.Type, transactionalAccount);
            incomeSource = _incomeSourceRepository.Add(incomeSource);
            await _incomeSourceRepository.UnitOfWork.SaveChangesAsync();
            return NewIncomeSourceAddedResponse.From(incomeSource);
        }

        private async Task<TransactionalAccount> AddAssociatedTransactionalAccountAsync(AddNewIncomeSourceCommand addNewLoanCommand)
        {
            AccountNumber incomeSourceSummaryAccountNumber = AccountNumber.From(SummaryAccount.DefaultAccountNumbers.IncomeSourcesAccount);
            AccountNumber incomeSourceAccountNumber = await _getNextAccountNumberService.GetNextAsync(incomeSourceSummaryAccountNumber);
            TransactionalAccount newIncomeSourceAccount = TransactionalAccount.NewRevenueAccount(accountNumber: incomeSourceAccountNumber, name: $"{addNewLoanCommand.Type}-{addNewLoanCommand.Name}", parentAccountNumber: incomeSourceSummaryAccountNumber);
            newIncomeSourceAccount = _transactionalAccountRepository.Add(newIncomeSourceAccount);
            return newIncomeSourceAccount;
        }
    }
}
