using Ardalis.GuardClauses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.LoanAggregate
{
    public class Loan : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string LoanProvider { get; set; }
        public Money LoanAmount { get; set; }
        public InterestRate InterestRate { get; set; }
        public AccountNumber AssociatedAccountNumber { get; private set; }

        public static class MaxLengths
        {
            public const int Name = 150;
            public const int LoanProvider = 100;
        }

        public static Loan From(string name, string loanProvider, Money loanAmount, InterestRate interestRate, TransactionalAccount associatedTransactionalAccount)
        {
            GuardAgainstInvalidName(name);
            GuardAgainstInvalidLoanProvider(loanProvider);
            Guard.Against.Null(loanAmount);
            Guard.Against.Null(interestRate);
            Guard.Against.Null(associatedTransactionalAccount);

            return new Loan
            {
                Name = name,
                LoanProvider = loanProvider,
                LoanAmount = loanAmount,
                InterestRate = interestRate,
                AssociatedAccountNumber = associatedTransactionalAccount.Number
            };
        }

        private static void GuardAgainstInvalidLoanProvider(string loanProvider)
        {
            Guard.Against.NullOrEmpty(loanProvider, nameof(loanProvider));
            Guard.Against.OutOfRange(loanProvider.Length, nameof(loanProvider), rangeFrom: 1, rangeTo: MaxLengths.LoanProvider);
        }

        private static void GuardAgainstInvalidName(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.OutOfRange(name.Length, nameof(name), rangeFrom: 1, rangeTo: MaxLengths.Name);
        }
    }
}
