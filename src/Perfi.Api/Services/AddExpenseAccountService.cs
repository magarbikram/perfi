using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Api.Services
{
    public class AddExpenseAccountService : IAddExpenseAccountService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;

        public AddExpenseAccountService(ITransactionalAccountRepository transactionalAccountRepository)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
        }

        public async Task<NewExpenseAccountAddedResponse> AddAsync(AddNewExpenseAccountCommand addNewExpenseAccountCommand)
        {
            AccountNumber homeExpensesSummaryAccountNumber = AccountNumber.From(SummaryAccount.DefaultAccountNumbers.HomeExpensesAccount);
            TransactionalAccount newHomeExpenseAccount = TransactionalAccount.NewExpenseAccount(number: addNewExpenseAccountCommand.Number, name: addNewExpenseAccountCommand.Name, parentAccountNumber: homeExpensesSummaryAccountNumber);
            newHomeExpenseAccount = _transactionalAccountRepository.Add(newHomeExpenseAccount);
            await _transactionalAccountRepository.UnitOfWork.SaveChangesAsync();
            return NewExpenseAccountAddedResponse.From(newHomeExpenseAccount);
        }
    }
}
