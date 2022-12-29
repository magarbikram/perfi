using Ardalis.GuardClauses;
using Perfi.Core.Earnings.IncomeSources;

namespace Perfi.Api.Responses
{
    public class ListIncomeSourceResponse
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public static ListIncomeSourceResponse From(IncomeSource incomeSource)
        {
            Guard.Against.Null(incomeSource);
            return new ListIncomeSourceResponse
            {
                Id = incomeSource.Id,
                Name = incomeSource.Name,
                Type = incomeSource.Type,
            };
        }
        public static IEnumerable<ListIncomeSourceResponse> From(IEnumerable<IncomeSource> incomeSources)
        {
            return incomeSources.Select(incomeSource => From(incomeSource));
        }
    }
}
