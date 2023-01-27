using CSharpFunctionalExtensions;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Payments.LoanPayments
{
    public class LoanPaymentMethod : ValueObject
    {
        public int CashAccountId { get; private set; }
        public AccountNumber CashAccountNumber { get; private set; }

        public static LoanPaymentMethod From(CashAccount cashAccount)
        {
            return new LoanPaymentMethod
            {
                CashAccountId = cashAccount.Id,
                CashAccountNumber = cashAccount.AssociatedAccountNumber
            };
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CashAccountId;
            yield return CashAccountNumber;
        }
    }
}
