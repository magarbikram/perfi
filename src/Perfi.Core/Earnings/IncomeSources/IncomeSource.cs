using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Earnings.IncomeSources
{
    public class IncomeSource : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public AccountNumber AssociatedAccountNumber { get; private set; }

        protected IncomeSource()
        {

        }

        public static IncomeSource From(string name, string type, TransactionalAccount transactionalAccount)
        {
            Guard.Against.NullOrEmpty(name);
            Guard.Against.OutOfRange(name.Length, nameof(name), rangeFrom: 1, rangeTo: MaxLengths.Name);


            Guard.Against.NullOrEmpty(type);
            Guard.Against.OutOfRange(type.Length, nameof(type), rangeFrom: 1, rangeTo: MaxLengths.Type);

            Guard.Against.Null(transactionalAccount);

            return new IncomeSource
            {
                Name = name,
                Type = type,
                AssociatedAccountNumber = transactionalAccount.Number
            };
        }

        public static class MaxLengths
        {
            public const int Name = 150;
            public const int Type = 50;
        }
    }
}
