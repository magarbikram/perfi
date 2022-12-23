using Perfi.Core.Accounts.Jobs;
using System.ComponentModel.DataAnnotations;

namespace Perfi.Api.Commands
{
    public class AddNewJobCommand
    {
        [Required]
        [MaxLength(Job.MaxLengths.JobHolder)]
        public string JobHolder { get; set; }

        [Required]
        [MaxLength(Job.MaxLengths.Employee)]
        public string Employee { get; set; }

        [MinLength(1)]
        public IEnumerable<JobIncomeSplitCommand> jobIncomeSplits { get; set; }
    }
}
