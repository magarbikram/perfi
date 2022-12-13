using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface ICreditCardAccountQueryService
    {
        Task<List<ListCreditCardAccountResponse>> GetAllAsync();
    }
}