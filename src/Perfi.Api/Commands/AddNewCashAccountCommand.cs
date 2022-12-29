using Perfi.Core.Accounts.CashAccountAggregate;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewCashAccountCommand
    {
        [Required]
        [MaxLength(CashAccount.MaxLengths.Name)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CashAccount.MaxLengths.BankName)]
        public string BankName { get; set; }
    }
}
