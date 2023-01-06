using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using System.Net.Http.Headers;

namespace Perfi.Core.Expenses
{
    public class ExpenseCategoryCode : ValueObject
    {
        public const int MaxLength = 50;

        public string Value { get; private set; }
        public static ExpenseCategoryCode Housing => From("Housing");
        public static ExpenseCategoryCode MortgagePayment => From("Mortgage");
        public static ExpenseCategoryCode Debts => From("Debts");
        public static ExpenseCategoryCode DebtPayment => From("Debt");

        protected ExpenseCategoryCode()
        {

        }
        public static ExpenseCategoryCode From(string value)
        {
            Guard.Against.NullOrEmpty(value, nameof(value));
            Guard.Against.OutOfRange(value.Length, nameof(value), rangeFrom: 1, rangeTo: MaxLength);
            return new ExpenseCategoryCode { Value = value };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}