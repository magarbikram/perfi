using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewExpenseAccountCommand
    {
        [Required]
        [MaxLength(CashAccount.MaxLengths.Name)]
        public string Name { get; set; }
    }
}
