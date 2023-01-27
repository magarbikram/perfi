using Perfi.Core.Accounting;
using Perfi.Core.Expenses;
using Perfi.Core.Payments.OutgoingPayments;

namespace Perfi.Api.Services
{
    public class AddOutgoingPaymentService : IAddOutgoingPaymentService
    {
        private readonly IOutgoingPaymentRepository _outgoingPaymentRepository;

        public AddOutgoingPaymentService(IOutgoingPaymentRepository outgoingPaymentRepository)
        {
            _outgoingPaymentRepository = outgoingPaymentRepository;
        }
        public void AddOutGoingPaymentFor(Expense expense, Money amount)
        {
            if (expense.PaymentMethod is not CashAccountExpensePaymentMethod)
            {
                return;
            }
            OutgoingPayment outgoingPayment = OutgoingPayment.From(description: $"Paid by cash for expense: {expense.Description} in category: {expense.ExpenseCategoryCode}",
                                                                   amount: amount,
                                                                   paidFromAccountNumber: expense.PaymentMethod.GetAssociatedAccountNumber(),
                                                                   transactionDate: expense.TransactionDate);
            _outgoingPaymentRepository.Add(outgoingPayment);
        }
    }
}
