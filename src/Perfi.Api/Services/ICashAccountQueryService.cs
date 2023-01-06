using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface ICashAccountQueryService
    {
        Task<List<ListCashAccountResponse>> GetAllAsync(bool withCurrentBalance);
    }
}