using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Api.Services
{
    public class AddCashAccountService : IAddCashAccountService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly ICashAccountRepository _cashAccountRepository;

        public AddCashAccountService(
            ITransactionalAccountRepository transactionalAccountRepository,
            ICashAccountRepository cashAccountRepository)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _cashAccountRepository = cashAccountRepository;
        }
        public async Task<NewCashAccountAddedResponse> AddAsync(AddNewCashAccountCommand addNewCashAccountCommand)
        {
            TransactionalAccount associatedTransactionalAccount = await AddAssociatedTransactionalAccountAsync(addNewCashAccountCommand);
            CashAccount cashAccount = CashAccount.From(addNewCashAccountCommand.Name, addNewCashAccountCommand.BankName, associatedTransactionalAccount);
            _cashAccountRepository.Add(cashAccount);
            await _cashAccountRepository.UnitOfWork.SaveChangesAsync();
            return NewCashAccountAddedResponse.From(cashAccount);
        }

        private async Task<TransactionalAccount> AddAssociatedTransactionalAccountAsync(AddNewCashAccountCommand addNewCashAccountCommand)
        {
            AccountNumber bankCashSummaryAccountNumber = AccountNumber.From(SummaryAccount.DefaultAccountNumbers.BankCashAccount);
            TransactionalAccount newBankCashAccount = TransactionalAccount.NewAssetAccount(number: addNewCashAccountCommand.Code, name: addNewCashAccountCommand.Name, parentAccountNumber: bankCashSummaryAccountNumber);
            newBankCashAccount = _transactionalAccountRepository.Add(newBankCashAccount);
            return newBankCashAccount;
        }
    }
}
