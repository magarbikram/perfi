using Ardalis.GuardClauses;
using Perfi.Core.MoneyTransfers;

namespace Perfi.Api.Responses
{
    public class ListMoneyTransferResponse
    {
        public int Id { get; private set; }
        public string Remarks { get; private set; }
        public MoneyResponse Amount { get; private set; }
        public string FromAccount { get; private set; }
        public string ToAccount { get; private set; }

        public static ListMoneyTransferResponse From(MoneyTransfer moneyTransfer)
        {
            Guard.Against.Null(moneyTransfer);
            return new ListMoneyTransferResponse
            {
                Id = moneyTransfer.Id,
                Remarks = moneyTransfer.Remarks,
                Amount = MoneyResponse.From(moneyTransfer.Amount),
                FromAccount = moneyTransfer.From,
                ToAccount = moneyTransfer.To,
            };
        }

        public static IEnumerable<ListMoneyTransferResponse> From(IEnumerable<MoneyTransfer> moneyTransfers)
        {
            return moneyTransfers.Select(mt => From(mt));
        }
    }
}
