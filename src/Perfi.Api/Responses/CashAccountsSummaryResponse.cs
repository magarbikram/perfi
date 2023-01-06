namespace Perfi.Api.Responses
{
    public class CashAccountsSummaryResponse
    {
        public MoneyResponse CurrentBalance { get; set; }
        public IEnumerable<ListCashAccountSummaryResponse> CashAccounts { get; set; }
    }
}
