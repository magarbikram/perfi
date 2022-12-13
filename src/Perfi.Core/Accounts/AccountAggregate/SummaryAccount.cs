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
            public const string CreditCardAccount = "200-01";
        }

        private List<Account> _components = new();
        public IReadOnlyCollection<Account> Components => _components.AsReadOnly();

        public void AddChildAccount(Account childAccount)
        {
            _components.Add(childAccount);
        }
    }
}
