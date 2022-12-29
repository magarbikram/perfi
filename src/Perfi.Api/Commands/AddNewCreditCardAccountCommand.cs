using Perfi.Core.Accounts.CreditCardAggregate;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewCreditCardAccountCommand
    {
        [Required]
        [MaxLength(CreditCardAccount.MaxLengths.Name)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CreditCardAccount.MaxLengths.CreditorName)]
        public string CreditorName { get; set; }

        [Required]
        [MaxLength(CreditCardAccount.MaxLengths.LastFourDigits)]
        public string LastFourDigits { get; set; }
    }
}
