using Ardalis.GuardClauses;

namespace Perfi.Core.Accounts.AccountAggregate
{
    public class TransactionalAccount : Account
    {
        protected TransactionalAccount()
        {

        }
        protected TransactionalAccount(AccountNumber accountNumber, string description, AccountCategory accountCategory) : base(accountNumber, description, accountCategory)
        {
        }

        public static TransactionalAccount NewAssetAccount(string number, string name, AccountNumber parentAccountNumber)
        {
            GuardAgainstInvalidName(name);
            return new(AccountNumber.From(number), name, AccountCategory.Assets)
            {
                ParentAccountNumber = parentAccountNumber
            };
        }
    }
}
