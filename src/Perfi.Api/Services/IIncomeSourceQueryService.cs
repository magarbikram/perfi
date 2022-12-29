using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IIncomeSourceQueryService
    {
        Task<IEnumerable<ListIncomeSourceResponse>> GetAllAsync();
    }
}