using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IPayLoanService
    {
        Task<NewExpenseAddedResponse> PayAsync(PayLoanCommand payLoanCommand);
    }
}