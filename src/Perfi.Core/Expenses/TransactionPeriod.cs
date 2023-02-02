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
            TimeSpan offset = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time").BaseUtcOffset;

            return For(DateTimeOffset.UtcNow.ToOffset(offset));
        }

        public static TransactionPeriod For(DateTimeOffset transactionDate)
        {
            return new TransactionPeriod
            {
                Value = transactionDate.ToString("MMMM, yyyy")
            };
        }

        public static TransactionPeriod For(int year, int month)
        {
            DateTimeOffset transactionPeriodDate = new(year, month, 1, 1, 1, 1, TimeSpan.Zero);
            return For(transactionPeriodDate);
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