using Ardalis.GuardClauses;
using Perfi.Core.Accounting;

namespace Perfi.Api.Responses
{
    public class MoneyResponse
    {
        public decimal Value { get; set; }
        public string Currency { get; set; }

        public static MoneyResponse From(Money money)
        {
            Guard.Against.Null(money, nameof(money));
            return new MoneyResponse { Value = money.Value.Value, Currency = money.Currency };
        }
    }
}
