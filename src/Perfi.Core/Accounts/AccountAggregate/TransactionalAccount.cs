using Ardalis.GuardClauses;
using Perfi.Core.Accounting;

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

        public void SetBeginingBalance(Money beginingBalanceAmount)
        {
            Guard.Against.Null(beginingBalanceAmount);
            BeginingBalance = beginingBalanceAmount;
        }

        public static class DefaultAccountNumbers
        {
            public const string InterestPaid = "500-01-01";
            public const string EscrowPaid = "500-01-02";
            public const string FeePaid = "500-01-03";

            public static AccountNumber GetInterestPaid()
            {
                return AccountNumber.From(InterestPaid);
            }

            public static AccountNumber GetEscrowPaid()
            {
                return AccountNumber.From(EscrowPaid);
            }

            public static AccountNumber GetFeePaid()
            {
                return AccountNumber.From(FeePaid);
            }
        }
    }
}
