using Ardalis.GuardClauses;
using Perfi.Core.Accounts.Jobs;

namespace Perfi.Api.Responses
{
    public class NewJobAddedResponse
    {
        public int Id { get; private set; }
        public string JobHolder { get; private set; }
        public string Employee { get; private set; }

        public static NewJobAddedResponse From(Job job)
        {
            Guard.Against.Null(job, nameof(job));
            return new NewJobAddedResponse { Id = job.Id, JobHolder = job.JobHolder, Employee = job.Employee };
        }
    }
}
