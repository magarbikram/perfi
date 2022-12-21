using Perfi.SharedKernel;
using Perfi.Core.Accounts.Exceptions;

namespace Perfi.Core.Accounting.AccountingTransactionAggregate
{
    public class AccountingTransaction : BaseEntity, IAggregateRoot
    {
        public const int DescriptionMaxLength = 255;
        public string Description { get; private set; }
        public DateTimeOffset TransactionDate { get; private set; }
        public DateTimeOffset DocumentDate { get; private set; }

        private readonly IList<AccountingEntry> _accountingEntries = new List<AccountingEntry>();
        public IEnumerable<AccountingEntry> AccountingEntries => _accountingEntries.AsEnumerable();

        private void AddEntries(params AccountingEntry[] accountingEntries)
        {
            GuardAgainstImbalancedTransactionEntries(accountingEntries);
            foreach (AccountingEntry entry in accountingEntries)
            {
                _accountingEntries.Add(entry);
            }
        }

        private static void GuardAgainstImbalancedTransactionEntries(AccountingEntry[] accountingEntries)
        {
            decimal totalDebitAmount = accountingEntries.Sum(ae => ae.DebitAmount?.Value ?? 0);
            decimal totalCreditAmount = accountingEntries.Sum(ae => ae.CreditAmount?.Value ?? 0);
            if (totalCreditAmount != totalDebitAmount)
            {
                throw new TransactionNotBalancedException($"Transaction should have balanced debit and credit amount");
            }
        }

        public static AccountingTransaction New(DateTimeOffset transactionDate, string description, params AccountingEntry[] accountingEntries)
        {
            AccountingTransaction accountingTransaction = new()
            {
                TransactionDate = transactionDate.UtcDateTime,
                DocumentDate = DateTimeOffset.UtcNow,
                Description = description,
            };
            accountingTransaction.AddEntries(accountingEntries);
            return accountingTransaction;
        }
    }
}
