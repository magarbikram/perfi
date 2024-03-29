﻿using Ardalis.GuardClauses;
using Perfi.Core.Accounts.CashAccountAggregate;

namespace Perfi.Api.Responses
{
    public class NewCashAccountAddedResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public static NewCashAccountAddedResponse From(CashAccount cashAccount)
        {
            Guard.Against.Null(cashAccount, nameof(cashAccount));
            return new NewCashAccountAddedResponse { Id = cashAccount.Id, Name = cashAccount.Name, BankName = cashAccount.BankName };
        }
    }
}
