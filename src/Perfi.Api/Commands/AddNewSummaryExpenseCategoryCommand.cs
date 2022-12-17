using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewSummaryExpenseCategoryCommand
    {
        [Required]
        [MaxLength(ExpenseCategoryCode.MaxLength)]
        public string Code { get; set; }

        [Required]
        [MaxLength(ExpenseCategory.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(AccountNumber.MaxLength)]
        public string ExpenseAccountNumber { get; set; }
    }
}
