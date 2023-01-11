using Ardalis.GuardClauses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Responses
{
    public class ListSplitPartnerWithCurrentBalanceResponse
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public MoneyResponse CurrentBalance { get; private set; }

        protected ListSplitPartnerWithCurrentBalanceResponse()
        {

        }

        public static ListSplitPartnerWithCurrentBalanceResponse From(SplitPartner splitPartner, Money currentBalance)
        {
            Guard.Against.Null(splitPartner);
            return new ListSplitPartnerWithCurrentBalanceResponse
            {
                Id = splitPartner.Id,
                Name = splitPartner.Name,
                CurrentBalance = MoneyResponse.From(currentBalance)
            };
        }

        public static async Task<IEnumerable<ListSplitPartnerWithCurrentBalanceResponse>> FromAsync(IEnumerable<SplitPartner> splitPartners, ICalculateCurrentBalanceService calculateCurrentBalanceService)
        {
            List<ListSplitPartnerWithCurrentBalanceResponse> responses = new();
            foreach (var splitPartner in splitPartners)
            {
                Money currentBalance = await calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(splitPartner.ReceivableAccountNumber);
                responses.Add(From(splitPartner, currentBalance));
            }
            return responses;
        }
    }
}
