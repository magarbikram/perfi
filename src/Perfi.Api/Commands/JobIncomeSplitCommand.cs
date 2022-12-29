using Perfi.Core.Accounts.AccountAggregate;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class JobIncomeSplitCommand : IValidatableObject
    {
        public int CashAccountId { get; set; }
        public bool SplitRemainderAmount { get; set; }
        public decimal? SplitAmount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SplitRemainderAmount && (!SplitAmount.HasValue || SplitAmount.Value <= 0))
            {
                yield return new ValidationResult(errorMessage: $"Split amount is required", new[] { nameof(SplitAmount) });
            }
        }
    }
}