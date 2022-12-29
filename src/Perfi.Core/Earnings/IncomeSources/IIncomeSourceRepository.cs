using CSharpFunctionalExtensions;
using Perfi.SharedKernel;

namespace Perfi.Core.Earnings.IncomeSources
{
    public interface IIncomeSourceRepository : IRepository<IncomeSource>
    {
        IncomeSource Add(IncomeSource incomeSource);
        Task<IEnumerable<IncomeSource>> GetAllAsync();
        Task<Maybe<IncomeSource>> GetByIdAsync(int id);
    }
}
