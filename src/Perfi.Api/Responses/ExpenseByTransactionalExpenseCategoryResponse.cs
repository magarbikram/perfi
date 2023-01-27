namespace Perfi.Api.Responses
{
    public class ExpenseByTransactionalExpenseCategoryResponse
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public MoneyResponse TotalExpenseAmount { get; set; }
    }
}