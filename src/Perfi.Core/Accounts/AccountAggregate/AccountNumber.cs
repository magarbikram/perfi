using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;

namespace Perfi.Core.Accounts.AccountAggregate
{
    public class AccountNumber : ValueObject
    {
        public const int MaxLength = 20;
        public string Value { get; private set; }

        public static AccountNumber From(string value)
        {
            Guard.Against.NullOrEmpty(value, nameof(value));
            Guard.Against.OutOfRange(value.Length, nameof(value), 1, MaxLength);
            return new() { Value = value };
        }

        public static AccountNumber Next(AccountNumber summaryAccountNumber, int count)
        {
            return From($"{summaryAccountNumber.Value}-{count + 1,2}");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}