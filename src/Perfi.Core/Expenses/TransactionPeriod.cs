using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;

namespace Perfi.Core.Expenses
{
    public class TransactionPeriod : ValueObject
    {
        public const int MaxLength = 20;
        public string Value { get; private set; }

        public static TransactionPeriod CurrentPeriod()
        {
            return For(DateTimeOffset.Now);
        }

        public static TransactionPeriod For(DateTimeOffset transactionDate)
        {
            return new TransactionPeriod
            {
                Value = transactionDate.ToString("MMMM, yyyy")
            };
        }

        public static TransactionPeriod From(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));
            Guard.Against.OutOfRange(value.Length, nameof(value), rangeFrom: 1, rangeTo: MaxLength);
            return new TransactionPeriod { Value = value };
        }

        public string GetMonthName()
        {
            return Value.Split(',')[0];
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}