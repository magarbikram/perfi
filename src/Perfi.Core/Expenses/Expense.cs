using Ardalis.GuardClauses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.SplitPartners;
using Perfi.SharedKernel;

namespace Perfi.Core.Expenses
{
    public class Expense : BaseEntity, IAggregateRoot
    {
        public const int DescriptionMaxLength = 255;
        public string Description { get; private set; }
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
        public ExpenseCategoryCode ExpenseCategoryCode { get; private set; }
        public Money Amount { get; private set; }
        public ExpensePaymentMethod PaymentMethod { get; private set; }
        public SplitPayment? SplitPayment { get; private set; }
        public int? AccountingTransactionId { get; private set; }

        protected Expense()
        {

        }

        public static Expense NewCreditCardExpense(
            string description, DateTimeOffset transactionDate, ExpenseCategoryCode expenseCategoryCode,
            Money amount, CreditCardAccount creditCardAccount)
        {
            Expense expense = new()
            {
                Description = description,
                TransactionDate = transactionDate.UtcDateTime,
                DocumentDate = DateTimeOffset.UtcNow,
                ExpenseCategoryCode = expenseCategoryCode,
                Amount = amount,
                PaymentMethod = CreditCardExpensePaymentMethod.From(creditCardAccount)
            };
            return expense;
        }

        public static Expense NewCashAccountExpense(
            string description, DateTimeOffset transactionDate, ExpenseCategoryCode expenseCategoryCode,
            Money amount, CashAccount cashAccount)
        {
            Expense expense = new()
            {
                Description = description,
                TransactionDate = transactionDate.UtcDateTime,
                DocumentDate = DateTimeOffset.UtcNow,
                ExpenseCategoryCode = expenseCategoryCode,
                Amount = amount,
                PaymentMethod = CashAccountExpensePaymentMethod.From(cashAccount)
            };
            return expense;
        }

        public void SetAccountingTransaction(AccountingTransaction accountingTransaction)
        {
            Guard.Against.Null(accountingTransaction, nameof(accountingTransaction));
            AccountingTransactionId = accountingTransaction.Id;
        }

        public static Expense NewSplitExpensePaidBySplitPartner(
            string description, DateTimeOffset transactionDate,
            ExpenseCategoryCode expenseCategoryCode, Money ownerShareExpenseAmount,
            SplitPartner splitPartner, Money splitPartnerShareExpenseAmount)
        {
            Expense expense = new()
            {
                Description = description,
                TransactionDate = transactionDate.UtcDateTime,
                DocumentDate = DateTimeOffset.UtcNow,
                ExpenseCategoryCode = expenseCategoryCode,
                Amount = ownerShareExpenseAmount + splitPartnerShareExpenseAmount,
                SplitPayment = SplitPayment.From(splitPartner, ownerShareExpenseAmount, splitPartnerShareExpenseAmount),
                PaymentMethod = SplitPartnerExpensePaymentMethod.From(splitPartner)
            };
            return expense;
        }

        public static Expense NewSplitExpenseWithCreditCardPayment(
            string description, DateTimeOffset transactionDate, ExpenseCategoryCode expenseCategoryCode, Money ownerShareExpenseAmount,
            CreditCardAccount creditCardAccount,
            SplitPartner splitPartner,
            Money splitPartnerShareExpenseAmount)
        {
            Expense expense = new()
            {
                Description = description,
                TransactionDate = transactionDate.UtcDateTime,
                DocumentDate = DateTimeOffset.UtcNow,
                ExpenseCategoryCode = expenseCategoryCode,
                Amount = ownerShareExpenseAmount + splitPartnerShareExpenseAmount,
                SplitPayment = SplitPayment.From(splitPartner, ownerShareExpenseAmount, splitPartnerShareExpenseAmount),
                PaymentMethod = CreditCardExpensePaymentMethod.From(creditCardAccount)
            };
            return expense;
        }

        public static Expense NewSplitExpenseWithCashAccountPayment(string description, DateTimeOffset transactionDate, ExpenseCategoryCode expenseCategoryCode,
            Money ownerShareExpenseAmount,
            CashAccount cashAccount,
            SplitPartner splitPartner,
            Money splitPartnerShareExpenseAmount)
        {
            Expense expense = new()
            {
                Description = description,
                TransactionDate = transactionDate.UtcDateTime,
                DocumentDate = DateTimeOffset.UtcNow,
                ExpenseCategoryCode = expenseCategoryCode,
                Amount = ownerShareExpenseAmount + splitPartnerShareExpenseAmount,
                SplitPayment = SplitPayment.From(splitPartner, ownerShareExpenseAmount, splitPartnerShareExpenseAmount),
                PaymentMethod = CashAccountExpensePaymentMethod.From(cashAccount)
            };
            return expense;
        }

        public bool IsSplit()
        {
            return SplitPayment != null;
        }
    }
}
