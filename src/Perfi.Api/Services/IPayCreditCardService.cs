using Perfi.Api.Commands;
using Perfi.Api.Responses;

namespace Perfi.Api.Services
{
    public interface IPayCreditCardService
    {
        Task<NewMoneyTransferResponse> PayAsync(PayCreditCardCommand payCreditCardCommand);
    }
}