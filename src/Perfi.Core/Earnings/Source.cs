using Ardalis.GuardClauses;
using Perfi.Core.Earnings.IncomeSources;

namespace Perfi.Core.Earnings
{
    public class Source
    {
        public string Name { get; private set; }
        public int IncomeSourceId { get; private set; }

        protected Source()
        {

        }
        public static Source From(IncomeSource incomeSource)
        {
            Guard.Against.Null(incomeSource);
            return new Source
            {
                Name = incomeSource.Name,
                IncomeSourceId = incomeSource.Id
            };
        }
    }
}