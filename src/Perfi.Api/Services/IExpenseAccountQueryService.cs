using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IExpenseAccountQueryService
    {
        Task<IEnumerable<ListExpenseAccountResponse>> GetAllAsync();
    }
}