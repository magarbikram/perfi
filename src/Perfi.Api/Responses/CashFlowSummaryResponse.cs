using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore.Query;
using Perfi.Core.Accounting;
using Perfi.Core.Expenses;

namespace Perfi.Api.Responses
{
    public class CashFlowSummaryResponse
    {
        public MoneyResponse TotalIncomeAmount { get; private set; }
        public MoneyResponse TotalExpenseAmount { get; private set; }
        public MoneyResponse CashFlowAmount { get; private set; }
        public string Name { get; private set; }


        internal static CashFlowSummaryResponse From(TransactionPeriod transactionPeriod, Money totalIncomeAmount, Money totalExpenseAmount)
        {
            Guard.Against.Null(totalIncomeAmount);
            Guard.Against.Null(totalExpenseAmount);
            return new CashFlowSummaryResponse
            {
                Name = transactionPeriod.Value,
                TotalExpenseAmount = MoneyResponse.From(totalExpenseAmount),
                TotalIncomeAmount = MoneyResponse.From(totalIncomeAmount),
                CashFlowAmount = MoneyResponse.From(totalIncomeAmount - totalExpenseAmount)
            };
        }
    }
}
