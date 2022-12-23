using Ardalis.GuardClauses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.Jobs
{
    public class JobIncomeSplit : BaseEntity
    {
        public AccountNumber CashAccountNumber { get; private set; }
        public Money? SplitAmount { get; private set; }
        public bool SplitRemainderAmount { get; private set; }
        protected JobIncomeSplit() { }

        public static JobIncomeSplit From(Money splitAmount, TransactionalAccount bankCashAccount)
        {
            Guard.Against.Null(nameof(splitAmount));
            GuardAgainstInvalidBankCashAccount(bankCashAccount);

            return new JobIncomeSplit
            {
                CashAccountNumber = bankCashAccount.Number,
                SplitAmount = splitAmount
            };
        }

        private static void GuardAgainstInvalidBankCashAccount(TransactionalAccount bankCashAccount)
        {
            Guard.Against.NullOrInvalidInput(bankCashAccount, nameof(bankCashAccount), predicate: IsBankCashAccount);
        }

        private static bool IsBankCashAccount(TransactionalAccount ca)
        {
            return ca.AccountCategory == AccountCategory.Assets &&
                   ca.ParentAccountNumber == AccountNumber.From(SummaryAccount.DefaultAccountNumbers.BankCashAccount);
        }

        public static JobIncomeSplit ForSplitRemainderAmount(TransactionalAccount bankCashAccount)
        {
            GuardAgainstInvalidBankCashAccount(bankCashAccount);
            return new JobIncomeSplit
            {
                CashAccountNumber = bankCashAccount.Number,
                SplitRemainderAmount = true
            };
        }
    }
}
