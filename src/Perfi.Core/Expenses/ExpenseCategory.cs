using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Expenses
{
    public abstract class ExpenseCategory
    {
        public ExpenseCategoryCode Code { get; set; }
        public string Name { get; set; }
    }

    public class SummaryExpenseCategory : ExpenseCategory
    {
        private List<TransactionalExpenseCategory> _subCategories = new();
        public IReadOnlyCollection<TransactionalExpenseCategory> SubCategories => _subCategories.AsReadOnly();
    }

    public class TransactionalExpenseCategory : ExpenseCategory
    {

    }
}
