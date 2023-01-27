using Perfi.Core.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Expenses.QueryModels
{
    public class ExpenseByCategory
    {
        public string ExpenseCategoryCode { get; set; }
        public string ExpenseCategoryName { get; set; }
        public string ParentExpenseCategoryCode { get; set; }
        public string ParentExpenseCategoryName { get; set; }
        public Money ExpenseAmount { get; set; }
    }
}
