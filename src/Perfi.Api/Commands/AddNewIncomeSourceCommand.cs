using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Earnings.IncomeSources;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewIncomeSourceCommand
    {
        [Required]
        [MaxLength(IncomeSource.MaxLengths.Name)]
        public string Name { get; set; }


        [Required]
        [MaxLength(IncomeSource.MaxLengths.Type)]
        public string Type { get; set; }
    }
}
