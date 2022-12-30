using Perfi.Core.Earnings;

namespace Perfi.Api.Responses
{
    public class NewIncomeAddedResponse
    {
        public int Id { get; private set; }
        public string Source { get; private set; }
        public MoneyResponse Amount { get; private set; }
        public IncomePaymentMethodResponse PaymentMethod { get; private set; }
        public long TransactionUnixTimeMilliseconds { get; private set; }
        internal static NewIncomeAddedResponse From(IncomeDocument incomeDocument)
        {
            return new NewIncomeAddedResponse
            {
                Id = incomeDocument.Id,
                Source = incomeDocument.Source.Name,
                Amount = MoneyResponse.From(incomeDocument.Amount),
                PaymentMethod = IncomePaymentMethodResponse.From(incomeDocument),
                TransactionUnixTimeMilliseconds = incomeDocument.TransactionDate.ToUnixTimeMilliseconds()
            };
        }
    }
}
