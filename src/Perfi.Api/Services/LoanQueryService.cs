using Perfi.Api.Responses;
using Perfi.Core.Accounts.LoanAggregate;

namespace Perfi.Api.Services
{
    public class LoanQueryService : ILoanQueryService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanQueryService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<List<ListLoanResponse>> GetAllAsync()
        {
            List<Loan> loans = await _loanRepository.GetAllAsync();
            return MapToResponses(loans);
        }

        private static List<ListLoanResponse> MapToResponses(List<Loan> loans)
        {
            List<ListLoanResponse> responses = new List<ListLoanResponse>();
            foreach (Loan loan in loans)
            {
                responses.Add(ListLoanResponse.From(loan));
            }
            return responses;
        }
    }
}
