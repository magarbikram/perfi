using Ardalis.GuardClauses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Earnings.IncomeSources;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Core.Earnings
{
    public class IncomeDocument : BaseEntity, IAggregateRoot
    {
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
        public Source Source { get; private set; }
        public PaymentDeposition PaymentDeposition { get; private set; }

        public Money Amount { get; private set; }
        public int? TransactionId { get; private set; }

        public static IncomeDocument NewJobIncome(IncomeSource incomeSource, Money incomeAmount, CashAccount cashAccountForDeposit, DateTimeOffset transactionDate)
        {
            Guard.Against.NullOrInvalidInput(incomeAmount, nameof(incomeAmount), (amount) => !amount.IsZeroOrLess());
            return new IncomeDocument
            {
                TransactionDate = transactionDate.ToUniversalTime(),
                DocumentDate = DateTimeOffset.UtcNow,
                Source = Source.From(incomeSource),
                PaymentDeposition = PaymenDepositionToCashAccount.From(cashAccountForDeposit),
                Amount = incomeAmount
            };
        }

        public void SetTransaction(AccountingTransaction transaction)
        {
            Guard.Against.Null(transaction);
            TransactionId = transaction.Id;
        }
    }
}
