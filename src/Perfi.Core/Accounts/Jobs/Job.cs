using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;

namespace Perfi.Core.Accounts.Jobs
{
    public class Job : BaseEntity, IAggregateRoot
    {
        private readonly List<JobIncomeSplit> _incomeSplits = new();

        public string Employee { get; private set; }
        public string JobHolder { get; private set; }
        public AccountNumber AssociatedAccountNumber { get; private set; }

        public IReadOnlyCollection<JobIncomeSplit> IncomeSplits => _incomeSplits.AsReadOnly();

        public static Job From(string jobHolder, string employee, TransactionalAccount jobServiceAccount)
        {
            GuardAgainstInvalidJobHolder(jobHolder);
            GuardAgainstInvalidEmployee(employee);
            GuardAgainstInvalidJobServiceAccount(jobServiceAccount);
            return new Job
            {
                JobHolder = jobHolder,
                Employee = employee,
                AssociatedAccountNumber = jobServiceAccount.Number
            };
        }

        private static void GuardAgainstInvalidJobServiceAccount(TransactionalAccount jobServiceAccount)
        {
            Guard.Against.Null(jobServiceAccount);
            if (jobServiceAccount.AccountCategory != AccountCategory.Revenues && jobServiceAccount.ParentAccountNumber != AccountNumber.From(SummaryAccount.DefaultAccountNumbers.JobServiceAccount))
            {
                throw new ArgumentException(message: $"Invalid job service account receid", paramName: nameof(jobServiceAccount));
            }
        }

        private static void GuardAgainstInvalidEmployee(string employee)
        {
            Guard.Against.NullOrEmpty(employee, nameof(employee));
            Guard.Against.OutOfRange(employee.Length, nameof(employee), rangeFrom: 1, rangeTo: MaxLengths.Employee);
        }

        private static void GuardAgainstInvalidJobHolder(string jobHolder)
        {
            Guard.Against.NullOrEmpty(jobHolder, nameof(jobHolder));
            Guard.Against.OutOfRange(jobHolder.Length, nameof(jobHolder), rangeFrom: 1, rangeTo: MaxLengths.JobHolder);
        }

        public void AddIncomeSplit(JobIncomeSplit jobIncomeSplit)
        {
            _incomeSplits.Add(jobIncomeSplit);
        }

        public static class MaxLengths
        {
            public const int Employee = 150;
            public const int JobHolder = 150;
        }
    }
}
