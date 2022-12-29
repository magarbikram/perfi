using CSharpFunctionalExtensions;

namespace Perfi.Core.Accounting
{
    public class Money : ValueObject
    {
        protected Money()
        {

        }

        public decimal Value { get; private set; }
        public string Currency { get; private set; }
        public static Money From(decimal value, string currency)
        {
            return new Money { Value = value, Currency = currency };
        }

        public static Money UsdFrom(decimal amount)
        {
            return From(amount, "USD");
        }

        public Money Clone()
        {
            return From(Value, Currency);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }

        public bool IsZero()
        {
            if (Value == decimal.Zero)
            {
                return true;
            }
            return false;
        }

        public static Money Max(Money x, Money y)
        {
            GuardAgainstMultipleCurrencies(x, y);
            if (x.Value > y.Value)
            {
                return x;
            }
            return y;
        }

        public static Money Min(Money x, Money y)
        {
            GuardAgainstMultipleCurrencies(x, y);
            if (x.Value < y.Value)
            {
                return x;
            }
            return y;
        }

        private static void GuardAgainstMultipleCurrencies(Money x, Money y)
        {
            if (x.Currency != y.Currency)
            {
                throw new ArgumentException($"Comparing money have different currencies");
            }
        }

        public bool IsZeroOrLess()
        {
            return Value <= decimal.Zero;
        }

        public static Money Sum(IEnumerable<Money> incomeAmounts)
        {
            Money sum = UsdFrom(0);
            foreach (Money money in incomeAmounts)
            {
                sum += money;
            }
            return sum;
        }

        public static Money operator -(Money x, Money y)
        {
            GuardAgainstMultipleCurrencies(x, y);
            return From(value: x.Value - y.Value, currency: y.Currency);
        }
        public static Money operator +(Money x, Money y)
        {
            GuardAgainstMultipleCurrencies(x, y);
            return From(value: x.Value + y.Value, currency: y.Currency);
        }
    }
}
