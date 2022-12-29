using Ardalis.GuardClauses;
using Perfi.Core.Accounting;

namespace Perfi.Api.Responses
{
    public class CashFlowSummaryResponse
    {
        public MoneyResponse TotalIncomeAmount { get; private set; }
        public MoneyResponse TotalExpenseAmount { get; private set; }

        internal static CashFlowSummaryResponse From(Money totalIncomeAmount, Money totalExpenseAmount)
        {
            Guard.Against.Null(totalIncomeAmount);
            Guard.Against.Null(totalExpenseAmount);
            return new CashFlowSummaryResponse
            {
                TotalExpenseAmount = MoneyResponse.From(totalExpenseAmount),
                TotalIncomeAmount = MoneyResponse.From(totalIncomeAmount)
            };
        }
    }
}
