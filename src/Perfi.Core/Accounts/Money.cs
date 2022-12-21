using CSharpFunctionalExtensions;

namespace Perfi.Core.Accounting
{
    public class Money : ValueObject
    {
        protected Money()
        {

        }

        public decimal? Value { get; private set; }
        public string Currency { get; private set; }

        public static Money From(decimal value, string currency)
        {
            return new Money { Value = value, Currency = currency };
        }

        public static Money UsdFrom(decimal amount)
        {
            return From(amount, "USD");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }
    }
}
