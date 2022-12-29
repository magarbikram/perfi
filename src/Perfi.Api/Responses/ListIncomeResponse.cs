using Perfi.Core.Earnings;

namespace Perfi.Api.Responses
{
    public class ListIncomeResponse
    {
        public int Id { get; private set; }
        public string Source { get; private set; }
        public MoneyResponse Amount { get; private set; }
        public IncomePaymentMethodResponse PaymentMethod { get; private set; }

        internal static ListIncomeResponse From(IncomeDocument incomeDocument)
        {
            return new ListIncomeResponse
            {
                Id = incomeDocument.Id,
                Source = incomeDocument.Source.Name,
                Amount = MoneyResponse.From(incomeDocument.Amount),
                PaymentMethod = IncomePaymentMethodResponse.From(incomeDocument)
            };
        }

        internal static IEnumerable<ListIncomeResponse> From(IEnumerable<IncomeDocument> currentIncomes)
        {
            return currentIncomes.Select(currentIncome => From(currentIncome));
        }
    }
}
