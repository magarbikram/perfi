using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewExpenseAccountCommand
    {
        /// <summary>
        /// Account Number
        /// </summary>
        [Required]
        [MaxLength(AccountNumber.MaxLength)]
        public string Number { get; set; }

        [Required]
        [MaxLength(CashAccount.MaxLengths.Name)]
        public string Name { get; set; }
    }
}
