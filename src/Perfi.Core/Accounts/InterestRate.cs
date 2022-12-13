using CSharpFunctionalExtensions;

namespace Perfi.Core.Accounting
{
    public class InterestRate : ValueObject
    {
        public decimal Value { get; set; }

        protected InterestRate()
        {

        }

        public static InterestRate From(decimal value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), message: $"should be greater than zero");
            }
            return new InterestRate { Value = value };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
