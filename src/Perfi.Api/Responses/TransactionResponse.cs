using Perfi.Core.Accounts.AccountingTransactionAggregate.QueryModels;

namespace Perfi.Api.Responses
{
    public class TransactionResponse
    {
        public int TransactionId { get; private set; }
        public long TransactionUnixTimeMilliseconds { get; private set; }
        public string Description { get; private set; }
        public MoneyResponse Amount { get; private set; }

        public static List<TransactionResponse> From(IEnumerable<Transaction> transactions)
        {
            return transactions.Select(tr => new TransactionResponse
            {
                TransactionId = tr.Id,
                TransactionUnixTimeMilliseconds = tr.TransactionDate.ToUnixTimeMilliseconds(),
                Description = tr.Description,
                Amount = MoneyResponse.From(tr.Amount)
            }).ToList();
        }
    }
}
