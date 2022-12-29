namespace Perfi.Api.Commands
{
    public partial class AddNewExpenseCommand
    {
        public string Description { get; set; }
        public long TransactionDateUnixTimeStamp { get; set; }
        public string ExpenseCategoryCode { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodCommand PaymentMethod { get; set; }
    }
}
