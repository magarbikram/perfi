
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Accounting.AccountingTransactionAggregate
{
    public class AccountingEntry : BaseEntity
    {
        public AccountingEntry(TransactionalAccount account, DateTimeOffset bookedTime, Money amount)
        {
            AccountNumber = account.Number;
            BookedTime = bookedTime;
            Amount = amount;
        }

        public AccountNumber AccountNumber { get; private set; }//must be detail account number and not summary account number
        public DateTimeOffset BookedTime { get; private set; }
        public Money Amount { get; private set; }

    }
}
