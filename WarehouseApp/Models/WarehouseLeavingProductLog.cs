using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp.Models
{
    internal class WarehouseLeavingProductLog
    {
        internal WarehouseIncomingProductLog recordIn;
        internal string truckName;
        internal int hourOut;
    }
}
