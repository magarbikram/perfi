using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class PayCreditCardCommand
    {
        [Required]
        public int CreditCardId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public decimal? FeeAmount { get; set; }

        [Required]
        public int PayWithCashAccountId { get; set; }

        [Required]
        public long TransactionUnixTimeMilliseconds { get; set; }
    }
}
