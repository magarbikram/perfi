using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.CreditCardAggregate
{
    public class CreditCardAccount : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string LastFourDigits { get; private set; }
        public string CreditorName { get; private set; }

        public static class MaxLengths
        {
            public const int Name = 150;
            public const int CreditorName = 100;
            public const int LastFourDigits = 4;
        }

        public AccountNumber AssociatedAccountNumber { get; private set; }

        public static CreditCardAccount From(string name, string creditorName, string lastFourDigit, TransactionalAccount associatedTransactionalAccount)
        {
            GuardAgainstInvalidName(name);
            GuardAgainstInvalidCreditorName(creditorName);
            GuardAgainstInvalidLastFourDigits(lastFourDigit);
            Guard.Against.Null(associatedTransactionalAccount);

            return new CreditCardAccount
            {
                Name = name,
                CreditorName = creditorName,
                LastFourDigits = lastFourDigit,
                AssociatedAccountNumber = associatedTransactionalAccount.Number
            };
        }

        private static void GuardAgainstInvalidLastFourDigits(string lastFourDigit)
        {
            Guard.Against.NullOrEmpty(lastFourDigit, nameof(lastFourDigit));
            Guard.Against.OutOfRange(lastFourDigit.Length, nameof(lastFourDigit), rangeFrom: 1, rangeTo: MaxLengths.LastFourDigits);
        }

        private static void GuardAgainstInvalidCreditorName(string creditorname)
        {
            Guard.Against.NullOrEmpty(creditorname, nameof(creditorname));
            Guard.Against.OutOfRange(creditorname.Length, nameof(creditorname), rangeFrom: 1, rangeTo: MaxLengths.CreditorName);
        }

        private static void GuardAgainstInvalidName(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.OutOfRange(name.Length, nameof(name), rangeFrom: 1, rangeTo: MaxLengths.Name);
        }
    }
}
