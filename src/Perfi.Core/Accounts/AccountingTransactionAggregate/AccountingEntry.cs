using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounting.AccountingTransactionAggregate
{
    public class AccountingEntry : BaseEntity
    {
        public DateTimeOffset DocumentDate { get; private set; }
        public DateTimeOffset TransactionDate { get; private set; }

        public AccountNumber AccountNumber { get; private set; }//must be detail account number and not summary account number
        public Money DebitAmount { get; private set; }
        public Money CreditAmount { get; private set; }

        public static AccountingEntry Credit(TransactionalAccount account, Money amount, DateTimeOffset transactionDate)
        {
            return new AccountingEntry
            {
                AccountNumber = account.Number,
                DocumentDate = DateTimeOffset.UtcNow,
                CreditAmount = amount,
                TransactionDate = transactionDate.UtcDateTime
            };
        }

        public static AccountingEntry Debit(TransactionalAccount account, Money amount, DateTimeOffset transactionDate)
        {
            return new AccountingEntry
            {
                AccountNumber = account.Number,
                DocumentDate = DateTimeOffset.UtcNow,
                DebitAmount = amount,

                TransactionDate = transactionDate.UtcDateTime
            };
        }
    }
}
