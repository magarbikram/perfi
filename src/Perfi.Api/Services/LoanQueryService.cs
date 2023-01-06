using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.LoanAggregate;

namespace Perfi.Api.Services
{
    public class LoanQueryService : ILoanQueryService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ICalculateCurrentBalanceService _calculateCurrentBalanceService;

        public LoanQueryService(
            ILoanRepository loanRepository,
            ICalculateCurrentBalanceService calculateCurrentBalanceService)
        {
            _loanRepository = loanRepository;
            _calculateCurrentBalanceService = calculateCurrentBalanceService;
        }

        public async Task<List<ListLoanResponse>> GetAllAsync(bool withCurrentBalance)
        {
            List<Loan> loans = await _loanRepository.GetAllAsync();
            return await MapToResponsesAsync(loans, withCurrentBalance);
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
