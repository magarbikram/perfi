using Perfi.Core.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Earnings
{
    public class IncomeDocument
    {
        public const int DescriptionMaxLength = 255;
        public string Description { get; private set; }

        private DateTimeOffset _transactionDate;
        public DateTimeOffset TransactionDate
        {
            get => _transactionDate; private set
            {
                _transactionDate = value;
                TransactionPeriod = TransactionPeriod.For(_transactionDate);
            }
        }
        public TransactionPeriod TransactionPeriod { get; private set; }
        public DateTimeOffset DocumentDate { get; private set; }
    }
}
