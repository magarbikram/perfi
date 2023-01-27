using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Expenses;
using Perfi.SharedKernel;

namespace Perfi.Core.Payments.LoanPayments
{
    public class LoanPayment : BaseEntity, IAggregateRoot
    {
        public int LoanId { get; set; }

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

        public LoanPaymentMethod PaymentMethod { get; private set; }
        public Money PrincipalAmount { get; private set; }
        public Money? InterestAmount { get; private set; }
        public Money? FeeAmount { get; private set; }
        public Money? EscrowAmount { get; private set; }

        public Money TotalPaymentAmount
        {
            get
            {
                Money totalAmount = PrincipalAmount;
                if (InterestAmount != null)
                {
                    totalAmount += InterestAmount;
                }
                if (FeeAmount != null)
                {
                    totalAmount += FeeAmount;
                }
                if (EscrowAmount != null)
                {
                    totalAmount += EscrowAmount;
                }
                return totalAmount;
            }
        }

        public int? TransactionId { get; private set; }

        public static LoanPayment NewMortgagePayment(int mortgageLoanId, LoanPaymentMethod loanPaymentMethod, Money principalAmount, Money? interestAmount, Money? escrowAmount, Money? feeAmount, DateTimeOffset transactionDate)
        {
            return new LoanPayment
            {
                LoanId = mortgageLoanId,
                PaymentMethod = loanPaymentMethod,
                PrincipalAmount = principalAmount,
                InterestAmount = interestAmount,
                EscrowAmount = escrowAmount,
                FeeAmount = feeAmount,
                TransactionDate = transactionDate.UtcDateTime,
                DocumentDate = DateTimeOffset.UtcNow
            };
        }

        public void SetAccountingTransaction(AccountingTransaction accountingTransaction)
        {
            TransactionId = accountingTransaction.Id;
        }

        public Money GetExpenseAmount()
        {
            Money expenseAmount = Money.UsdFrom(decimal.Zero);
            if (InterestAmount != null)
            {
                expenseAmount += InterestAmount;
            }
            if (EscrowAmount != null)
            {
                expenseAmount += EscrowAmount;
            }
            if (FeeAmount != null)
            {
                expenseAmount += FeeAmount;
            }
            return expenseAmount;
        }

        public bool HasNoExpenseWith()
        {
            return InterestAmount == null && EscrowAmount == null && FeeAmount == null;
        }
    }
}
