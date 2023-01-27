using Perfi.Api.Responses;
using Perfi.Core.Expenses;
using Perfi.Core.MoneyTransfers;

namespace Perfi.Api.Services
{
    public class MoneyTransferQueryService : IMoneyTransferQueryService
    {
        private readonly IMoneyTransferRepository _moneyTransferRepository;

        public MoneyTransferQueryService(IMoneyTransferRepository moneyTransferRepository)
        {
            _moneyTransferRepository = moneyTransferRepository;
        }
        public async Task<IEnumerable<ListMoneyTransferResponse>> GetAllTransfersAsync(TransactionPeriod transactionPeriod)
        {
            IEnumerable<MoneyTransfer> moneyTransfers = await _moneyTransferRepository.GetAllAsync(transactionPeriod);
            return ListMoneyTransferResponse.From(moneyTransfers);
        }

        public async Task<IEnumerable<ListMoneyTransferResponse>> GetLimitedTransfersAsync(TransactionPeriod transactionPeriod, int count)
        {
            IEnumerable<MoneyTransfer> moneyTransfers = await _moneyTransferRepository.GetLimitedTransfersAsync(transactionPeriod, count);
            return ListMoneyTransferResponse.From(moneyTransfers);
        }
    }
}
