using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp.Models
{
    internal class WarehouseIncomingProductLog
    {
        internal string factoryName;
        internal double quantity;
        internal int hourIn;
        internal Product product;
        internal bool isLeave;
    }
}
