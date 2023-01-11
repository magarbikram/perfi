using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.SplitPartners
{
    public class SplitPartner : BaseEntity, IAggregateRoot
    {
        public const int NameMaxLength = 150;
        public string Name { get; private set; }
        public AccountNumber ReceivableAccountNumber { get; private set; }

        public SplitPartner()
        {

        }
        public static SplitPartner From(string name)
        {
            Guard.Against.NullOrEmpty(name);
            Guard.Against.OutOfRange(name.Length, name, rangeFrom: 1, rangeTo: NameMaxLength);
            return new SplitPartner
            {
                Name = name
            };
        }

        public void SetAccountsPayable(TransactionalAccount splitPartnerAccountReceivable)
        {
            Guard.Against.Null(splitPartnerAccountReceivable);
            ReceivableAccountNumber = splitPartnerAccountReceivable.Number;
        }
    }
}
