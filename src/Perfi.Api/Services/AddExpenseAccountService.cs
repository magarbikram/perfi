using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Api.Services
{
    public class AddExpenseAccountService : IAddExpenseAccountService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly IGetNextAccountNumberService _getNextAccountNumberService;

        public AddExpenseAccountService(
            ITransactionalAccountRepository transactionalAccountRepository,
            IGetNextAccountNumberService getNextAccountNumberService)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _getNextAccountNumberService = getNextAccountNumberService;
        }

        public async Task<NewExpenseAccountAddedResponse> AddAsync(AddNewExpenseAccountCommand addNewExpenseAccountCommand)
        {
            AccountNumber homeExpensesSummaryAccountNumber = AccountNumber.From(SummaryAccount.DefaultAccountNumbers.HomeExpensesAccount);
            AccountNumber newExpenseAccountNumber = await _getNextAccountNumberService.GetNextAsync(homeExpensesSummaryAccountNumber);
            TransactionalAccount newHomeExpenseAccount = TransactionalAccount.NewExpenseAccount(newExpenseAccountNumber, name: addNewExpenseAccountCommand.Name, parentAccountNumber: homeExpensesSummaryAccountNumber);
            newHomeExpenseAccount = _transactionalAccountRepository.Add(newHomeExpenseAccount);
            await _transactionalAccountRepository.UnitOfWork.SaveChangesAsync();
            return NewExpenseAccountAddedResponse.From(newHomeExpenseAccount);
        }
    }
}
