using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class PayMortgageCommand
    {
        [Required]
        public int MortgageLoanId { get; set; }

        [Required]
        public decimal PrincipalAmount { get; set; }

        [Required]
        public decimal InterestAmount { get; set; }

        [Required]
        public decimal EscrowAmount { get; set; }

        public decimal? FeeAmount { get; set; }

        [Required]
        public PaymentMethodCommand PaymentMethod { get; set; }

        [Required]
        public long TransactionUnixTimeMilliseconds { get; set; }
    }
}
