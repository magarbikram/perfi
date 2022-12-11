using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Core.Accounting
{
    public class Money
    {
        public Money(int v1, string v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public int V1 { get; }
        public string V2 { get; }
    }
}
