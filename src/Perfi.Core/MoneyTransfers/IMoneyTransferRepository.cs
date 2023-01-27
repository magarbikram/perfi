using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Core.MoneyTransfers
{
    public interface IMoneyTransferRepository : IRepository<MoneyTransfer>
    {
        MoneyTransfer Add(MoneyTransfer moneyTransfer);
        Task<IEnumerable<MoneyTransfer>> GetAllAsync(TransactionPeriod transactionPeriod);
        Task<IEnumerable<MoneyTransfer>> GetLimitedTransfersAsync(TransactionPeriod transactionPeriod, int count);
        void Update(MoneyTransfer moneyTransfer);
    }
}
