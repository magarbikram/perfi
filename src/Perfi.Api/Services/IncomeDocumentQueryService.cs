using Perfi.Api.Models;
using Perfi.Api.Responses;
using Perfi.Core.Earnings;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class IncomeDocumentQueryService : IIncomeDocumentQueryService
    {
        private readonly IIncomeDocumentRepository _incomeDocumentRepository;

        public IncomeDocumentQueryService(IIncomeDocumentRepository incomeDocumentRepository)
        {
            _incomeDocumentRepository = incomeDocumentRepository;
        }
        public async Task<IEnumerable<ListIncomeResponse>> GetCurrentIncomesAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.For(DateTimeOffset.Now);
            IEnumerable<IncomeDocument> currentIncomes = await _incomeDocumentRepository.GetAllForTransactionPeriodAsync(transactionPeriod: currentTransactionPeriod);
            return MapToResponses(currentIncomes);
        }

        private IEnumerable<ListIncomeResponse> MapToResponses(IEnumerable<IncomeDocument> currentIncomes)
        {
            return ListIncomeResponse.From(currentIncomes);
        }

        public async Task<IEnumerable<ListIncomeResponse>> GetCurrentTop10IncomesAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.For(DateTimeOffset.Now);
            IEnumerable<IncomeDocument> currentIncomes = await _incomeDocumentRepository.GetTop10IncomesForTransactionPeriodAsync(transactionPeriod: currentTransactionPeriod);
            return MapToResponses(currentIncomes);
        }
    }
}
