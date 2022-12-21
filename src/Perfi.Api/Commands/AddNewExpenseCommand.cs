using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewExpenseCommand
    {
        public string Description { get; set; }
        public long TransactionDateUnixTimeStamp { get; set; }
        public string ExpenseCategoryCode { get; set; }
        public decimal Amount { get; set; }
        public ExpensePaymentMethod PaymentMethod { get; set; }

        public class ExpensePaymentMethod : IValidatableObject
        {
            public PaymentMethodType PaymentMethodType { get; set; }
            public int? CashAccountId { get; set; }
            public int? CreditCardAccountId { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (PaymentMethodType == PaymentMethodType.CashAccount)
                {
                    if (!CashAccountId.HasValue)
                    {
                        yield return new ValidationResult(errorMessage: $"Please provide cash account id");
                    }
                }
                else if (PaymentMethodType == PaymentMethodType.CreditCard)
                {
                    if (!CreditCardAccountId.HasValue)
                    {
                        yield return new ValidationResult(errorMessage: $"Please provide credit card account id");
                    }
                }
                else
                {
                    yield return new ValidationResult(errorMessage: $"Please provide valid payment method type(cashaccount or creditcard)");
                }
            }
        }
    }
}
