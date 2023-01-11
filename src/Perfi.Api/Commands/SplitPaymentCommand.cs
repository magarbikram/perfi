using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class SplitPaymentCommand
    {
        public bool IsPaidBySplitPartner { get; set; }
        public int SplitPartnerId { get; set; }

        [Required]
        public PaymentShareCommand PaymentShare { get; set; }
    }
}