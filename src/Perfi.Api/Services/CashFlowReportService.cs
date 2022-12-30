using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Earnings;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class CashFlowReportService : ICashFlowReportService
    {
        private readonly IIncomeDocumentRepository _incomeDocumentRepository;
        private readonly IExpenseRepository _expenseRepository;

        public CashFlowReportService(
            IIncomeDocumentRepository incomeDocumentRepository,
            IExpenseRepository expenseRepository)
        {
            _incomeDocumentRepository = incomeDocumentRepository;
            _expenseRepository = expenseRepository;
        }

        public async Task<CashFlowSummaryResponse> GetCurrentPeriodCashFlowSummaryAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.CurrentPeriod();
            Money totalIncomeAmount = await _incomeDocumentRepository.GetTotalIncomeAmountForPeriodAsync(currentTransactionPeriod);
            Money totalExpenseAmount = await _expenseRepository.GetTotalExpenseAmountForPeriodAsync(currentTransactionPeriod);

            return CashFlowSummaryResponse.From(currentTransactionPeriod, totalIncomeAmount, totalExpenseAmount);
        }
    }
}
