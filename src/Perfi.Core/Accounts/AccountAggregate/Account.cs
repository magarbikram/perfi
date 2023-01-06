using Ardalis.GuardClauses;
using Perfi.Core.Accounting;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.AccountAggregate
{
    public abstract class Account : BaseEntity, IAggregateRoot
    {
        public const int NameMaxLength = 150;

        protected Account()
        {

        }
        protected Account(AccountNumber accountNumber, string name, AccountCategory accountCategory)
        {
            Number = accountNumber;
            Name = name;
            AccountCategory = accountCategory;
        }
        public static void GuardAgainstInvalidName(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.OutOfRange(name.Length, nameof(name), rangeFrom: 1, rangeTo: NameMaxLength);
        }
        public AccountCategory AccountCategory { get; }

        public AccountNumber Number { get; private set; }
        public string Name { get; private set; }

        public AccountNumber ParentAccountNumber { get; protected set; }

        public Money? BeginingBalance { get; protected set; }
    }
}
