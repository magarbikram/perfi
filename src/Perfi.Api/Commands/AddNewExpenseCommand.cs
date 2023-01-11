using Perfi.Core.Expenses;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewExpenseCommand : IValidatableObject
    {
        [Required]
        [MaxLength(Expense.DescriptionMaxLength)]
        public string Description { get; set; }
        public long TransactionDateUnixTimeStamp { get; set; }

        [Required]
        [MaxLength(Core.Expenses.ExpenseCategoryCode.MaxLength)]
        public string ExpenseCategoryCode { get; set; }

        public decimal Amount { get; set; }
        public PaymentMethodCommand? PaymentMethod { get; set; }
        public SplitPaymentCommand? SplitPayment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsPaidBySplitPartner() && HasNoPaymentMethod())
            {
                yield return new ValidationResult("Payment method is required", new[] { nameof(PaymentMethod) });
            }
            if (!IsSplitExpense() && HasNoAmount())
            {
                yield return new ValidationResult("Amount is required", new[] { nameof(Amount) });
            }
        }

        private bool HasNoAmount()
        {
            return Amount <= decimal.Zero;
        }

        private bool HasNoPaymentMethod()
        {
            return PaymentMethod == null;
        }

        private bool IsPaidBySplitPartner()
        {
            return IsSplitExpense() && SplitPayment.IsPaidBySplitPartner;
        }

        internal bool IsSplitExpense()
        {
            return SplitPayment != null;
        }
    }
}
