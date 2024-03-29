﻿using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
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

        public async Task<CashFlowSummaryResponse> GetCashFlowSummaryAsync(TransactionPeriod transactionPeriod)
        {
            Money totalIncomingAmount = await _incomingPaymentRepository.GetTotalAmountAsync(transactionPeriod);
            Money totalOutgoingAmount = await _outgoingPaymentRepository.GetTotalAmountAsync(transactionPeriod);

            return CashFlowSummaryResponse.From(transactionPeriod, totalIncomingAmount, totalOutgoingAmount);
        }

        public async Task<CashFlowSummaryResponse> GetCurrentPeriodCashFlowSummaryAsync()
        {
            return await GetCashFlowSummaryAsync(TransactionPeriod.CurrentPeriod());
        }
    }
}
