using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Earnings;
using Perfi.Core.Expenses;
using Perfi.Core.MoneyTransfers;
using Perfi.Core.Payments.IncomingPayments;
using Perfi.Core.Payments.OutgoingPayments;

namespace Perfi.Api.Services
{
    public class CashFlowReportService : ICashFlowReportService
    {
        private readonly IIncomingPaymentRepository _incomingPaymentRepository;
        private readonly IOutgoingPaymentRepository _outgoingPaymentRepository;

        public CashFlowReportService(
            IIncomingPaymentRepository incomingPaymentRepository,
            IOutgoingPaymentRepository outgoingPaymentRepository)
        {
            _incomingPaymentRepository = incomingPaymentRepository;
            _outgoingPaymentRepository = outgoingPaymentRepository;
        }

        public async Task<CashFlowSummaryResponse> GetCurrentPeriodCashFlowSummaryAsync()
        {
            TransactionPeriod currentTransactionPeriod = TransactionPeriod.CurrentPeriod();
            Money totalIncomingAmount = await _incomingPaymentRepository.GetTotalAmountAsync(currentTransactionPeriod);
            Money totalOutgoingAmount = await _outgoingPaymentRepository.GetTotalAmountAsync(currentTransactionPeriod);

            return CashFlowSummaryResponse.From(currentTransactionPeriod, totalIncomingAmount, totalOutgoingAmount);
        }
    }
}
