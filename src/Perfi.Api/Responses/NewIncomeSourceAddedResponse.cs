using Ardalis.GuardClauses;
using Perfi.Core.Earnings.IncomeSources;

namespace Perfi.Api.Responses
{
    public class NewIncomeSourceAddedResponse
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public static NewIncomeSourceAddedResponse From(IncomeSource incomeSource)
        {
            Guard.Against.Null(incomeSource);
            return new NewIncomeSourceAddedResponse
            {
                Id = incomeSource.Id,
                Name = incomeSource.Name,
                Type = incomeSource.Type,
            };
        }
    }
}
