namespace Perfi.Api.Commands
{
    public class AddNewIncomeCommand
    {
        public int IncomeSourceId { get; set; }
        public decimal Amount { get; set; }
        public int CashAccountIdToDeposit { get; set; }
        public long TransactionUnixTimeMilliseconds { get; set; }
    }
}
