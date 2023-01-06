using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;

namespace Perfi.Api.Services
{
    public class AddCreditCardAccountService : IAddCreditCardAccountService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly IGetNextAccountNumberService _getNextAccountNumberService;

        public AddCreditCardAccountService(
            ITransactionalAccountRepository transactionalAccountRepository,
            ICreditCardAccountRepository cashAccountRepository,
            IGetNextAccountNumberService getNextAccountNumberService)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _creditCardAccountRepository = cashAccountRepository;
            _getNextAccountNumberService = getNextAccountNumberService;
        }
        public async Task<NewCreditCardAccountAddedResponse> AddAsync(AddNewCreditCardAccountCommand addNewCreditCardAccountCommand)
        {
            TransactionalAccount associatedTransactionalAccount = await AddAssociatedTransactionalAccountAsync(addNewCreditCardAccountCommand);
            CreditCardAccount creditCardAccount = CreditCardAccount.From(addNewCreditCardAccountCommand.Name, addNewCreditCardAccountCommand.CreditorName, addNewCreditCardAccountCommand.LastFourDigits, associatedTransactionalAccount);
            _creditCardAccountRepository.Add(creditCardAccount);
            await _creditCardAccountRepository.UnitOfWork.SaveChangesAsync();
            return NewCreditCardAccountAddedResponse.From(creditCardAccount);
        }

        private async Task<TransactionalAccount> AddAssociatedTransactionalAccountAsync(AddNewCreditCardAccountCommand addNewCreditCardAccountCommand)
        {
            AccountNumber creditCardSummaryAccountNumber = AccountNumber.From(SummaryAccount.DefaultAccountNumbers.CreditCardAccount);
            AccountNumber newCreditCardAccountNumber = await _getNextAccountNumberService.GetNextAsync(creditCardSummaryAccountNumber);
            TransactionalAccount newCreditCardAccount = TransactionalAccount.NewLiabilityAccount(newCreditCardAccountNumber, name: addNewCreditCardAccountCommand.Name, parentAccountNumber: creditCardSummaryAccountNumber);
            if (addNewCreditCardAccountCommand.CurrentBalance.HasValue)
            {
                newCreditCardAccount.SetBeginingBalance(Money.UsdFrom(addNewCreditCardAccountCommand.CurrentBalance.Value).Negate());
            }
            newCreditCardAccount = _transactionalAccountRepository.Add(newCreditCardAccount);
            return newCreditCardAccount;
        }
    }
}
