using Ardalis.GuardClauses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Core.MoneyTransfers
{
    public class MoneyTransfer : BaseEntity, IAggregateRoot
    {
        public string Remarks { get; private set; }
        public AccountNumber FromAccountNumber { get; private set; }
        public string From { get; private set; }

        public AccountNumber ToAccountNumber { get; private set; }
        public string To { get; private set; }
        public Money Amount { get; private set; }
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
        public DateTimeOffset DocumentDate { get; private set; }

        public int? AccountingTransactionId { get; private set; }

        protected MoneyTransfer()
        {

        }

        public static MoneyTransfer New(string remarks, Money amount, AccountNumber fromAccountNumber, string from, AccountNumber toAccountNumber, string to, DateTimeOffset transactionDate)
        {
            Guard.Against.NullOrWhiteSpace(remarks);
            Guard.Against.Null(amount);
            Guard.Against.Null(fromAccountNumber);
            Guard.Against.NullOrWhiteSpace(from);
            Guard.Against.Null(toAccountNumber);
            Guard.Against.NullOrWhiteSpace(to);

            return new MoneyTransfer
            {
                Remarks = remarks,
                Amount = amount,
                FromAccountNumber = fromAccountNumber,
                From = from,
                ToAccountNumber = toAccountNumber,
                To = to,
                TransactionDate = transactionDate.UtcDateTime
            };
        }

        public void SetAccountingTransaction(AccountingTransaction accountingTransaction)
        {
            Guard.Against.Null(accountingTransaction);
            AccountingTransactionId = accountingTransaction.Id;
        }

        public static class MaxLengths
        {
            public const int Remarks = 250;
            public const int From = 150;
            public const int To = 150;
        }
    }
}
