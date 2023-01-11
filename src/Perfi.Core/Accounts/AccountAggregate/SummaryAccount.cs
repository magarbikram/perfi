namespace Perfi.Core.Accounts.AccountAggregate
{
    public class SummaryAccount : Account
    {
        protected SummaryAccount()
        {

        }
        protected SummaryAccount(AccountNumber accountNumber, string description, AccountCategory accountCategory) : base(accountNumber, description, accountCategory)
        {
        }

        public static class DefaultAccountNumbers
        {
            public const string BankCashAccount = "100-01";
            public const string ReceivableAccount = "100-02";
            public const string IncomeSourcesAccount = "400-01";
            public const string CreditCardAccount = "200-01";
            public const string LoanAccount = "200-02";
            public const string HomeExpensesAccount = "300-01";
            public const string EquityAccount = "500-01";

            public static AccountNumber GetReceivableAccountNumber() => AccountNumber.From(ReceivableAccount);

        }

        private List<Account> _components = new();
        public IReadOnlyCollection<Account> Components => _components.AsReadOnly();

        public void AddChildAccount(Account childAccount)
        {
            _components.Add(childAccount);
        }

        public static SummaryAccount Asset(string name, string accountNumber)
        {
            return new SummaryAccount(AccountNumber.From(accountNumber), name, AccountCategory.Assets);
        }

        public static SummaryAccount Liability(string name, string accountNumber)
        {
            return new SummaryAccount(AccountNumber.From(accountNumber), name, AccountCategory.Liabilities);
        }

        public static SummaryAccount Expense(string name, string accountNumber)
        {
            return new SummaryAccount(AccountNumber.From(accountNumber), name, AccountCategory.Expenses);
        }

        public static SummaryAccount Revenue(string name, string accountNumber)
        {
            return new SummaryAccount(AccountNumber.From(accountNumber), name, AccountCategory.Revenues);
        }
    }
}
