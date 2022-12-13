﻿using Ardalis.GuardClauses;
using Perfi.Core.Accounts.LoanAggregate;

namespace Perfi.Api.Responses
{
    public class ListLoanResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LoanProvider { get; set; }
        public decimal InterestRate { get; set; }
        public MoneyResponse LoanAmount { get; set; }
        public static ListLoanResponse From(Loan loan)
        {
            Guard.Against.Null(loan, nameof(loan));
            return new ListLoanResponse { Id = loan.Id, Name = loan.Name, LoanProvider = loan.LoanProvider, InterestRate = loan.InterestRate.Value, LoanAmount = MoneyResponse.From(loan.LoanAmount) };
        }
    }
}
