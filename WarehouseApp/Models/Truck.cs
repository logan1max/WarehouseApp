using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp.Models
{
    internal class Truck
    {
        internal string name;
        internal double capacity;

        internal Truck(string name, double capacity)
        {
            this.name = name;
            this.capacity = capacity;
        }
    }
}
