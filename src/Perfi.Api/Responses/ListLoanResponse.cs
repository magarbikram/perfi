using Ardalis.GuardClauses;
using Perfi.Core.Accounts.LoanAggregate;

namespace Perfi.Api.Responses
{
    public class ListLoanResponse
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string LoanProvider { get; private set; }
        public decimal InterestRate { get; private set; }
        public MoneyResponse LoanAmount { get; private set; }
        public MoneyResponse CurrentBalance { get; private set; }

        public static ListLoanResponse From(Loan loan, Core.Accounting.Money currentBalance)
        {
            Guard.Against.Null(currentBalance);
            ListLoanResponse listLoanResponse = From(loan);
            listLoanResponse.CurrentBalance = MoneyResponse.From(currentBalance);
            return listLoanResponse;
        }

        internal static ListLoanResponse From(Loan loan)
        {
            Guard.Against.Null(loan, nameof(loan));
            return new ListLoanResponse { Id = loan.Id, Name = loan.Name, LoanProvider = loan.LoanProvider, InterestRate = loan.InterestRate.Value, LoanAmount = MoneyResponse.From(loan.LoanAmount) };
        }
    }
}
