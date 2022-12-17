using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Expenses
{
    public abstract class ExpenseCategory : BaseEntity, IAggregateRoot
    {
        public const int NameMaxLength = 150;

        public ExpenseCategoryCode Code { get; protected set; }
        public string Name { get; protected set; }
        public AccountNumber AssociatedExpenseAccountNumber { get; protected set; }

        protected static void ValidateAssocaitedExpenseAccount(TransactionalAccount associatedExpenseAccount)
        {
            Guard.Against.Null(associatedExpenseAccount);
            if (associatedExpenseAccount.AccountCategory != AccountCategory.Expenses)
            {
                throw new ArgumentException(message: $"It must of of type Expenses", nameof(associatedExpenseAccount));
            }
        }

        protected static void ValidateName(string name)
        {
            Guard.Against.NullOrEmpty(name);
            Guard.Against.OutOfRange(name.Length, nameof(name), rangeFrom: 1, rangeTo: ExpenseCategory.NameMaxLength);
        }
    }
}
