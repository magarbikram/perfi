using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Accounts.Jobs
{
    public interface IJobRepository : IRepository<Job>
    {
        Job Add(Job job);
    }
}
