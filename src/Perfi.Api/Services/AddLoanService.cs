using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.LoanAggregate;

namespace Perfi.Api.Services
{
    public class AddLoanService : IAddLoanService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IGetNextAccountNumberService _getNextAccountNumberService;

        public AddLoanService(
            ITransactionalAccountRepository transactionalAccountRepository,
            ILoanRepository loanRepository,
            IGetNextAccountNumberService getNextAccountNumberService)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _loanRepository = loanRepository;
            _getNextAccountNumberService = getNextAccountNumberService;
        }
        public async Task<NewLoanAddedResponse> AddAsync(AddNewLoanCommand addNewLoanCommand)
        {
            TransactionalAccount associatedTransactionalAccount = await AddAssociatedTransactionalAccountAsync(addNewLoanCommand);
            Loan loan = Loan.From(addNewLoanCommand.Name, addNewLoanCommand.LoanProvider, loanAmount: Money.From(addNewLoanCommand.LoanAmount, "USD"), InterestRate.From(addNewLoanCommand.InterestRate), associatedTransactionalAccount);
            _loanRepository.Add(loan);
            await _loanRepository.UnitOfWork.SaveChangesAsync();
            return NewLoanAddedResponse.From(loan);
        }

        private async Task<TransactionalAccount> AddAssociatedTransactionalAccountAsync(AddNewLoanCommand addNewLoanCommand)
        {
            AccountNumber bankCashSummaryAccountNumber = AccountNumber.From(SummaryAccount.DefaultAccountNumbers.LoanAccount);
            AccountNumber newLoanAccountNumber = await _getNextAccountNumberService.GetNextAsync(bankCashSummaryAccountNumber);
            TransactionalAccount newBankLoan = TransactionalAccount.NewLiabilityAccount(newLoanAccountNumber, name: addNewLoanCommand.Name, parentAccountNumber: bankCashSummaryAccountNumber);
            newBankLoan.SetBeginingBalance(Money.UsdFrom(addNewLoanCommand.LoanAmount).Negate());
            newBankLoan = _transactionalAccountRepository.Add(newBankLoan);
            return newBankLoan;
        }
    }
}
