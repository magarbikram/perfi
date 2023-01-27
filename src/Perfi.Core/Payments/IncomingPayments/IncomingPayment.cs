using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Earnings;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Core.Payments.IncomingPayments
{
    public class IncomingPayment : BaseEntity, IAggregateRoot
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
        public string Description { get; private set; }
        public Money Amount { get; private set; }
        public AccountNumber DepositedToAccountNumber { get; private set; }

        public static IncomingPayment From(IncomeDocument incomeDocument)
        {
            return new IncomingPayment
            {
                TransactionDate = incomeDocument.TransactionDate,
                Amount = incomeDocument.Amount,
                DepositedToAccountNumber = incomeDocument.PaymentDeposition.GetAssociatedAccountNumber(),
                Description = $"Incoming payment from income document with source: {incomeDocument.Source.Name}"
            };
        }

        public static IncomingPayment From(string description, Money amount, AccountNumber depositedToAccountNumber, DateTimeOffset transactionDate)
        {
            return new IncomingPayment
            {
                TransactionDate = transactionDate,
                Amount = amount,
                DepositedToAccountNumber = depositedToAccountNumber,
                Description = description
            };
        }
    }
}
