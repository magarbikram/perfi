namespace Perfi.Api.Responses
{
    public class SummaryCategoryExpenseReportResponse
    {
        public string SummaryCategoryCode { get; set; }
        public string SummaryCategoryName { get; set; }
        public MoneyResponse TotalExpenseAmount { get; set; }
    }
}
