using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;

namespace Perfi.Api.Services
{
    public class AddCreditCardAccountService : IAddCreditCardAccountService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;

        public AddCreditCardAccountService(
            ITransactionalAccountRepository transactionalAccountRepository,
            ICreditCardAccountRepository cashAccountRepository)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _creditCardAccountRepository = cashAccountRepository;
        }
        public async Task<NewCreditCardAccountAddedResponse> AddAsync(AddNewCreditCardAccountCommand addNewCreditCardAccountCommand)
        {
            TransactionalAccount associatedTransactionalAccount = AddAssociatedTransactionalAccount(addNewCreditCardAccountCommand);
            CreditCardAccount creditCardAccount = CreditCardAccount.From(addNewCreditCardAccountCommand.Name, addNewCreditCardAccountCommand.CreditorName, addNewCreditCardAccountCommand.LastFourDigits, associatedTransactionalAccount);
            _creditCardAccountRepository.Add(creditCardAccount);
            await _creditCardAccountRepository.UnitOfWork.SaveChangesAsync();
            return NewCreditCardAccountAddedResponse.From(creditCardAccount);
        }

        private TransactionalAccount AddAssociatedTransactionalAccount(AddNewCreditCardAccountCommand addNewCreditCardAccountCommand)
        {
            AccountNumber CreditCardSummaryAccountNumber = AccountNumber.From(SummaryAccount.DefaultAccountNumbers.CreditCardAccount);
            TransactionalAccount newCreditCardAccount = TransactionalAccount.NewAssetAccount(number: addNewCreditCardAccountCommand.Code, name: addNewCreditCardAccountCommand.Name, parentAccountNumber: CreditCardSummaryAccountNumber);
            newCreditCardAccount = _transactionalAccountRepository.Add(newCreditCardAccount);
            return newCreditCardAccount;
        }
    }
}
