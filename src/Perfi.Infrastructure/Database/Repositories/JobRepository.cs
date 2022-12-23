using Perfi.Core.Accounts.Jobs;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;
        public JobRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Job Add(Job job)
        {
            return _appDbContext.Jobs.Add(job).Entity;
        }
    }
}
