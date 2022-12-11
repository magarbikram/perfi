using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.CashAccountAggregate
{
    public class CashAccount : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string BankName { get; private set; }

        public static class MaxLengths
        {
            public const int Name = 150;
            public const int BankName = 100;
        }

        public AccountNumber AssociatedAccountNumber { get; private set; }

        public static CashAccount From(string name, string bankName, TransactionalAccount associatedTransactionalAccount)
        {
            GuardAgainstInvalid(name);
            GuardAgainstInvalidBank(bankName);
            Guard.Against.Null(associatedTransactionalAccount);

            return new CashAccount
            {
                Name = name,
                BankName = bankName,
                AssociatedAccountNumber = associatedTransactionalAccount.Number
            };
        }

        private static void GuardAgainstInvalidBank(string bankName)
        {
            Guard.Against.NullOrEmpty(bankName, nameof(bankName));
            Guard.Against.OutOfRange(bankName.Length, nameof(bankName), rangeFrom: 1, rangeTo: MaxLengths.BankName);
        }

        private static void GuardAgainstInvalid(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.OutOfRange(name.Length, nameof(name), rangeFrom: 1, rangeTo: MaxLengths.Name);
        }
    }
}
