using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Perfi.Core.Accounting.AccountingTransactionAggregate
{
    public class AccountingTransaction : BaseEntity, IAggregateRoot
    {
        private IList<AccountingEntry> _accountingEntries = new List<AccountingEntry>();
        public IEnumerable<AccountingEntry> AccountingEntries => _accountingEntries.AsEnumerable();

        public void AddEntries(params AccountingEntry[] accountingEntries)
        {
            foreach (AccountingEntry entry in accountingEntries)
            {
                _accountingEntries.Add(entry);
            }
        }

        //Sum of accounting entries must have a zero balance
    }
}
