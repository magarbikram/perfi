using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;
using Perfi.Core.Payments.LoanPayments;
using Perfi.SharedKernel;

namespace Perfi.Core.Payments.OutgoingPayments
{
    public class OutgoingPayment : BaseEntity, IAggregateRoot
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
        public AccountNumber PaidFromAccountNumber { get; private set; }

        public static OutgoingPayment From(string description, Money amount, AccountNumber paidFromAccountNumber, DateTimeOffset transactionDate)
        {
            return new OutgoingPayment
            {
                Description = description,
                Amount = amount,
                PaidFromAccountNumber = paidFromAccountNumber,
                TransactionDate = transactionDate
            };
        }

        public static OutgoingPayment From(LoanPayment loanPayment)
        {
            return new OutgoingPayment
            {
                Description = $"Repayment for loan with id '{loanPayment.LoanId}'",
                Amount = loanPayment.TotalPaymentAmount,
                PaidFromAccountNumber = loanPayment.PaymentMethod.CashAccountNumber,
                TransactionDate = loanPayment.TransactionDate
            };
        }
    }
}
