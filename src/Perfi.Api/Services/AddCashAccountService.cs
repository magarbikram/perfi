using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Api.Services
{
    public class AddCashAccountService : IAddCashAccountService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly IGetNextAccountNumberService _getNextAccountNumberService;

        public AddCashAccountService(
            ITransactionalAccountRepository transactionalAccountRepository,
            ICashAccountRepository cashAccountRepository,
            IGetNextAccountNumberService getNextAccountNumberService)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _cashAccountRepository = cashAccountRepository;
            _getNextAccountNumberService = getNextAccountNumberService;
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
            AccountNumber newCashAccountNumber = await _getNextAccountNumberService.GetNextAsync(bankCashSummaryAccountNumber);
            TransactionalAccount newBankCashAccount = TransactionalAccount.NewAssetAccount(newCashAccountNumber, name: addNewCashAccountCommand.Name, parentAccountNumber: bankCashSummaryAccountNumber);
            if (addNewCashAccountCommand.CurrentBalance.HasValue && addNewCashAccountCommand.CurrentBalance > 0)
            {
                newBankCashAccount.SetBeginingBalance(Money.UsdFrom(addNewCashAccountCommand.CurrentBalance.Value));
            }
            newBankCashAccount = _transactionalAccountRepository.Add(newBankCashAccount);
            return newBankCashAccount;
        }
    }
}
