namespace Perfi.Api.Responses
{
    public class AccountSummaryResponse
    {
        public MoneyResponse CashAccountsBalance { get; set; }
        public MoneyResponse CreditCardAccountsBalance { get; set; }
        public MoneyResponse LoansBalance { get; set; }
        public MoneyResponse InvestmentsBalance { get; set; }
    }
}
