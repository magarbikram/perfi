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

        public AddLoanService(
            ITransactionalAccountRepository transactionalAccountRepository,
            ILoanRepository loanRepository)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _loanRepository = loanRepository;
        }
        public async Task<NewLoanAddedResponse> AddAsync(AddNewLoanCommand addNewLoanCommand)
        {
            TransactionalAccount associatedTransactionalAccount = AddAssociatedTransactionalAccountAsync(addNewLoanCommand);
            Loan loan = Loan.From(addNewLoanCommand.Name, addNewLoanCommand.LoanProvider, loanAmount: Money.From(addNewLoanCommand.LoanAmount, "USD"), InterestRate.From(addNewLoanCommand.InterestRate), associatedTransactionalAccount);
            _loanRepository.Add(loan);
            await _loanRepository.UnitOfWork.SaveChangesAsync();
            return NewLoanAddedResponse.From(loan);
        }

        private TransactionalAccount AddAssociatedTransactionalAccountAsync(AddNewLoanCommand addNewLoanCommand)
        {
            AccountNumber bankCashSummaryAccountNumber = AccountNumber.From(SummaryAccount.DefaultAccountNumbers.LoanAccount);
            TransactionalAccount newBankLoan = TransactionalAccount.NewAssetAccount(number: addNewLoanCommand.Code, name: addNewLoanCommand.Name, parentAccountNumber: bankCashSummaryAccountNumber);
            newBankLoan = _transactionalAccountRepository.Add(newBankLoan);
            return newBankLoan;
        }
    }
}
