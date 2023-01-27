namespace Perfi.Api.Responses
{
    public class ListExpenseByCategoryResponse
    {
        public IEnumerable<ExpenseBySummaryCategoryResponse> SummaryCategories { get; set; }
    }
}
