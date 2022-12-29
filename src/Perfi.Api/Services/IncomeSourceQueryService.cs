using Perfi.Api.Responses;
using Perfi.Core.Earnings.IncomeSources;

namespace Perfi.Api.Services
{
    public class IncomeSourceQueryService : IIncomeSourceQueryService
    {
        private readonly IIncomeSourceRepository _incomeSourceRepository;

        public IncomeSourceQueryService(IIncomeSourceRepository incomeSourceRepository)
        {
            _incomeSourceRepository = incomeSourceRepository;
        }
        public async Task<IEnumerable<ListIncomeSourceResponse>> GetAllAsync()
        {
            IEnumerable<IncomeSource> incomeSources = await _incomeSourceRepository.GetAllAsync();
            return ListIncomeSourceResponse.From(incomeSources);
        }
    }
}
