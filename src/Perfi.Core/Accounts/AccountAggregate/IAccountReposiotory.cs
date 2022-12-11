using Perfi.Core.Accounts.AccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Accounting
{
    public interface IAccountReposiotory
    {
        public void Add(Account account);
        public void SaveChanges();

        /*
         Id,    AccountNumber,          Description                                          Type               ParentAccountNumber
         -----------------------        -------------                                       ----------         ----------------------
          1        100-001              Liability                                           Liability                 Null
          2        100-002              Asset                                               Asset                     Null
          3        100-003              Income                                              Income                    Null
          4        100-004              Expense                                             Expense                   Null
          5        100-001-001          Credit Cards                                        Liability                100-001    
          6        100-001-001-001      BoFA Cash Reward Credit Card - 7182 (Bikram)        Liability                100-001-001
          6        100-001-001-002      BoFA Travel Reward Credit Card - 4704 (Bikram)      Liability                100-001-001
         */

        /*
         Id,    AccountNumber,          Description                                          Type               ParentAccountNumber
         -----------------------        -------------                                       ----------         ----------------------
          1        100-001-001          Credit Cards                                        Liability                   Null    
          2        100-001-001-001      BoFA Cash Reward Credit Card - 7182 (Bikram)        Liability                100-001-001
          3        100-001-001-002      BoFA Travel Reward Credit Card - 4704 (Bikram)      Liability                100-001-001
         */
    }
}
