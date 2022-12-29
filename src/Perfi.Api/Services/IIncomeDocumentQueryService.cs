using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IIncomeDocumentQueryService
    {
        Task<IEnumerable<ListIncomeResponse>> GetCurrentIncomesAsync();
        Task<IEnumerable<ListIncomeResponse>> GetCurrentTop10IncomesAsync();
    }
}