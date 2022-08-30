using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp.Models
{
    internal class Factory
    {
        internal string name;
        internal double speed;
        internal Product product;

        internal Factory(string name, double speed)
        {
            this.name = name;
            this.speed = speed;

            Random random = new Random();

            product = new Product()
            {
                name = "Product" + name.ToString(),
                weight = random.Next(100, 1000),
                packageType = random.Next(10, 15)
            };            
        }
    }
}
