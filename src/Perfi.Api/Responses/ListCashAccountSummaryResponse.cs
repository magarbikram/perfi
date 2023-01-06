using Ardalis.GuardClauses;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Api.Responses
{
    public class ListCashAccountSummaryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public MoneyResponse CurrentBalance { get; set; }
        public static async Task<ListCashAccountSummaryResponse> FromAsync(CashAccount cashAccount, ICalculateCurrentBalanceService calculateCurrentBalanceService)
        {
            Guard.Against.Null(cashAccount, nameof(cashAccount));
            return new ListCashAccountSummaryResponse
            {
                Id = cashAccount.Id,
                Name = cashAccount.Name,
                BankName = cashAccount.BankName,
                CurrentBalance = await CalculateCurrentBalance(cashAccount, calculateCurrentBalanceService)
            };
        }

        private static async Task<MoneyResponse> CalculateCurrentBalance(CashAccount cashAccount, ICalculateCurrentBalanceService calculateCurrentBalanceService)
        {
            return MoneyResponse.From(await calculateCurrentBalanceService.GetCurrentBalanceOfAccountAsync(cashAccount.AssociatedAccountNumber));
        }
    }
}
