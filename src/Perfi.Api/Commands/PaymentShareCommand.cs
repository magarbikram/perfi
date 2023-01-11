using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class PaymentShareCommand
    {
        [Required]
        [Range(0.1, double.MaxValue)]
        public decimal OwnerShare { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SplitPartnerShare { get; set; }
    }
}