using Perfi.Core.Accounts.LoanAggregate;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class TransferAccount : IValidatableObject
    {
        public TransferAccountType Type { get; set; }
        public int? CashAccountId { get; set; }
        public int? SplitPartnerId { get; set; }
        public int? CreditCardId { get; set; }
        public int? LoanId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type == TransferAccountType.CashAccount && (!CashAccountId.HasValue || CashAccountId.Value <= 0))
            {
                yield return new ValidationResult($"CashAccountId is required", new string[] { nameof(CashAccountId) });
            }
            if (Type == TransferAccountType.SplitPartner && (!SplitPartnerId.HasValue || SplitPartnerId.Value <= 0))
            {
                yield return new ValidationResult($"SplitPartnerId is required", new string[] { nameof(SplitPartnerId) });
            }
            if (Type == TransferAccountType.CreditCard && (!CreditCardId.HasValue || CreditCardId.Value <= 0))
            {
                yield return new ValidationResult($"CreditCardId is required", new string[] { nameof(CreditCardId) });
            }
            if (Type == TransferAccountType.Loan && (!LoanId.HasValue || LoanId.Value <= 0))
            {
                yield return new ValidationResult($"LoanId is required", new string[] { nameof(LoanId) });
            }
        }
    }
}