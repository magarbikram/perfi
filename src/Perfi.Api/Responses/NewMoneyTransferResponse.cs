using Ardalis.GuardClauses;
using Perfi.Core.MoneyTransfers;

namespace Perfi.Api.Responses
{
    public class NewMoneyTransferResponse
    {
        public int Id { get; private set; }
        public string Remarks { get; private set; }
        public MoneyResponse Amount { get; private set; }
        public string FromAccount { get; private set; }
        public string ToAccount { get; private set; }
        internal static NewMoneyTransferResponse From(MoneyTransfer moneyTransfer)
        {
            Guard.Against.Null(moneyTransfer);
            return new NewMoneyTransferResponse
            {
                Id = moneyTransfer.Id,
                Remarks = moneyTransfer.Remarks,
                Amount = MoneyResponse.From(moneyTransfer.Amount),
                FromAccount = moneyTransfer.From,
                ToAccount = moneyTransfer.To,
            };
        }
    }
}
