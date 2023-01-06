using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IPayMortgageService
    {
        Task<NewExpenseAddedResponse> PayAsync(PayMortgageCommand payMortgageCommand);
    }
}