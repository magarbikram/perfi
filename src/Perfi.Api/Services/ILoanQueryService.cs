using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface ILoanQueryService
    {
        Task<List<ListLoanResponse>> GetAllAsync();
    }
}