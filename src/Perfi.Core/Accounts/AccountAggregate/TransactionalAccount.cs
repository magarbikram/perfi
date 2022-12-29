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

        public static TransactionalAccount NewAssetAccount(AccountNumber accountNumber, string name, AccountNumber parentAccountNumber)
        {
            GuardAgainstInvalidName(name);
            return new(accountNumber, name, AccountCategory.Assets)
            {
                ParentAccountNumber = parentAccountNumber
            };
        }

        public static TransactionalAccount NewLiabilityAccount(AccountNumber accountNumber, string name, AccountNumber parentAccountNumber)
        {
            GuardAgainstInvalidName(name);
            return new(accountNumber, name, AccountCategory.Liabilities)
            {
                ParentAccountNumber = parentAccountNumber
            };
        }

        public static TransactionalAccount NewExpenseAccount(AccountNumber accountNumber, string name, AccountNumber parentAccountNumber)
        {
            Guard.Against.Null(accountNumber);
            GuardAgainstInvalidName(name);
            return new(accountNumber, name, AccountCategory.Expenses)
            {
                ParentAccountNumber = parentAccountNumber
            };
        }

        public static TransactionalAccount NewRevenueAccount(AccountNumber accountNumber, string name, AccountNumber parentAccountNumber)
        {
            GuardAgainstInvalidName(name);
            return new(accountNumber, name, AccountCategory.Revenues)
            {
                ParentAccountNumber = parentAccountNumber
            };
        }
    }
}
