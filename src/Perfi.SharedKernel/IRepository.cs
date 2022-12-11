using System;
using System.Collections.Generic;
using System.Text;

namespace Perfi.SharedKernel
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
