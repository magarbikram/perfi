using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class PayLoanCommand
    {
        [Required]
        public int LoanId { get; set; }

        [Required]
        public decimal PrincipalAmount { get; set; }

        [Required]
        public decimal InterestAmount { get; set; }

        public decimal? FeeAmount { get; set; }

        [Required]
        public PaymentMethodCommand PaymentMethod { get; set; }

        [Required]
        public long TransactionUnixTimeMilliseconds { get; set; }
    }
}
