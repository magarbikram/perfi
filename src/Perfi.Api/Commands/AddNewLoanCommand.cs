using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewLoanCommand : IValidatableObject
    {
        [Required]
        [MaxLength(AccountNumber.MaxLength)]
        public string Code { get; set; }

        [Required]
        [MaxLength(Loan.MaxLengths.Name)]
        public string Name { get; set; }

        [Required]
        [MaxLength(Loan.MaxLengths.LoanProvider)]
        public string LoanProvider { get; set; }

        [Required]
        public decimal InterestRate { get; set; }

        [Required]
        public decimal LoanAmount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (InterestRate <= 0)
            {
                yield return new ValidationResult($"InterestRate should be a positive value");
            }
            if (LoanAmount <= 0)
            {
                yield return new ValidationResult($"LoanAmount should be a positive value");
            }
        }
    }
}
