using Perfi.Core.MoneyTransfers;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class TransferMoneyCommand
    {
        [Required]
        [MaxLength(MoneyTransfer.MaxLengths.Remarks)]
        public string Remarks { get; set; }

        public decimal Amount { get; set; }

        public TransferAccount FromAccount { get; set; }
        public TransferAccount ToAccount { get; set; }

        public long TransactionUnixTimeMilliseconds { get; set; }
    }
}
