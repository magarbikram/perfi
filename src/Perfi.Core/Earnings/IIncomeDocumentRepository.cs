using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Core.Earnings
{
    public interface IIncomeDocumentRepository : IRepository<IncomeDocument>
    {
        IncomeDocument Add(IncomeDocument incomeDocument);
        Task<Money> GetTotalIncomeAmountForPeriodAsync(TransactionPeriod currentTransactionPeriod);
        Task<IEnumerable<IncomeDocument>> GetAllForTransactionPeriodAsync(TransactionPeriod transactionPeriod);
        Task<IEnumerable<IncomeDocument>> GetTop10IncomesForTransactionPeriodAsync(TransactionPeriod transactionPeriod);
        void Update(IncomeDocument incomeDocument);
    }
}
