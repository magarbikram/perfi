using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;
using System.Security.Principal;

namespace Perfi.Core.Accounting.AccountingTransactionAggregate
{
    public class AccountingEntry : BaseEntity
    {
        public DateTimeOffset DocumentDate { get; private set; }
        private DateTimeOffset _transactionDate;

        public DateTimeOffset TransactionDate
        {
            get => _transactionDate; private set
            {
                _transactionDate = value;
                TransactionPeriod = TransactionPeriod.For(_transactionDate);
            }
        }
        public TransactionPeriod TransactionPeriod { get; private set; }

        public AccountNumber AccountNumber { get; private set; }//must be detail account number and not summary account number
        public Money? DebitAmount { get; private set; }
        public Money? CreditAmount { get; private set; }

        public static AccountingEntry Credit(TransactionalAccount account, Money amount, DateTimeOffset transactionDate)
        {
            return Credit(account.Number, amount, transactionDate);
        }

        public static AccountingEntry Credit(AccountNumber associatedAccountNumber, Money amount, DateTimeOffset transactionDate)
        {
            return new AccountingEntry
            {
                AccountNumber = associatedAccountNumber,
                DocumentDate = DateTimeOffset.UtcNow,
                CreditAmount = amount,
                TransactionDate = transactionDate.UtcDateTime
            };
        }

        public static AccountingEntry Debit(TransactionalAccount account, Money amount, DateTimeOffset transactionDate)
        {
            return Debit(account.Number, amount, transactionDate);
        }

        public static AccountingEntry Debit(AccountNumber accountNumber, Money amount, DateTimeOffset transactionDate)
        {
            return new AccountingEntry
            {
                AccountNumber = accountNumber,
                DocumentDate = DateTimeOffset.UtcNow,
                DebitAmount = amount,

                TransactionDate = transactionDate.UtcDateTime
            };
        }
    }
}
