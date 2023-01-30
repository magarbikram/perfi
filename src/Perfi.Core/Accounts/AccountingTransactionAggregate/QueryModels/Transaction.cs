using Perfi.Core.Accounting;

namespace Perfi.Core.Accounts.AccountingTransactionAggregate.QueryModels
{
    public class Transaction
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public DateTimeOffset TransactionDate { get; private set; }
        public Money Amount { get; private set; }

        public static Transaction From(int id, string description, DateTimeOffset transactionDate, Money? debitAmount, Money? creditAmount)
        {
            return new Transaction
            {
                Description = description,
                Id = id,
                TransactionDate = transactionDate,
                Amount = CalculateAmount(debitAmount, creditAmount)
            };
        }

        private static Money CalculateAmount(Money? debitAmount, Money? creditAmount)
        {
            Money amount = Money.UsdFrom(0);
            if (debitAmount != null)
            {
                amount += debitAmount!;
            }
            if (creditAmount != null)
            {
                amount -= creditAmount!;
            }

            return amount;
        }
    }
}
