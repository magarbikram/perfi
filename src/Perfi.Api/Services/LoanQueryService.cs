using CSharpFunctionalExtensions;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate.QueryModels;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class LoanQueryService : ILoanQueryService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;

        public LoanQueryService(
            ILoanRepository loanRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService,
            IAccountingTransactionRepository accountingTransactionRepository)
        {
            _loanRepository = loanRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
            _accountingTransactionRepository = accountingTransactionRepository;
        }

        public async Task<List<ListLoanResponse>> GetAllAsync(bool withCurrentBalance)
        {
            List<Loan> loans = await _loanRepository.GetAllAsync();
            return await MapToResponsesAsync(loans, withCurrentBalance);
        }
        public async Task<List<TransactionResponse>> GetAllTransactionsAsync(int creditCardId, TransactionPeriod transactionPeriod)
        {
            Maybe<Loan> maybeLoan = await _loanRepository.GetByIdAsync(creditCardId);
            if (maybeLoan.HasNoValue)
            {
                throw new ResourceNotFoundException($"Loan with id '{creditCardId}' not found");
            }
            Loan loan = maybeLoan.Value;
            IEnumerable<Transaction> transactions = await _accountingTransactionRepository.GetAccountingTransactionsOfPeriodAsync(loan.AssociatedAccountNumber, transactionPeriod);
            return TransactionResponse.From(transactions);

        }
        private async Task<List<ListLoanResponse>> MapToResponsesAsync(List<Loan> loans, bool withCurrentBalance)
        {
            List<ListLoanResponse> responses = new List<ListLoanResponse>();
            foreach (Loan loan in loans)
            {
                if (withCurrentBalance)
                {
                    Money currentBalance = await _calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(loan.AssociatedAccountNumber);
                    responses.Add(ListLoanResponse.From(loan, currentBalance));
                }
                else
                {
                    responses.Add(ListLoanResponse.From(loan));
                }

            }
            return responses;
        }
    }
}
