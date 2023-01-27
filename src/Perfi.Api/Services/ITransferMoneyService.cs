using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface ITransferMoneyService
    {
        Task<NewMoneyTransferResponse> TransferAsync(TransferMoneyCommand transferMoneyCommand);
    }
}