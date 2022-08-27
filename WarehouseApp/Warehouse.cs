using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseApp.Models;

namespace WarehouseApp
{
    internal class Warehouse
    {
        int M;
        int n;
        int k; //кол-во заводов

        double perHour; //кол-во продукции в час

        double warehouseCapacity; //вместимость склада

        List<Factory> factories;
        List<Truck> trucks;

        internal Warehouse()
        {
            factories = new List<Factory>();
            trucks = new List<Truck>() { new Truck("First", 1000), new Truck("Second", 3000) };

            M = 100;
            n = 50;
            k = 10;

            double koef = 1.0;

            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int i = 0; i < k; i++)
            {
                Factory f = new Factory(alphabet.Substring(i, 1), koef * n);

                factories.Add(f);
                koef += 0.1;
            }

            perHour = (from f in factories.AsParallel()
                       select f).Sum(f => f.speed);

            warehouseCapacity = M * perHour;
        }

        internal async Task Work()
        {
            int hours = 1; //кол-во часов работы

            List<WarehouseProductLog> log = new List<WarehouseProductLog>();

            while (hours <= 20)
            {                
                Console.WriteLine("Время: " + hours);

                //var temp = from f in factories.AsParallel()
                //           select new
                //           {
                //               factoryName = f.name,
                //               quantity = f.speed,
                //               hour = hours,
                //               product = f.product
                //            };

                //var temp = from f in factories
                //           select new WarehouseProductLog
                //           {
                //               factoryName = f.name,
                //               quantity = f.speed,
                //               hour = hours,
                //               product = f.product
                //           };

                var temp = factories.AsParallel()
                    .Select(x => new WarehouseProductLog
                    {
                        factoryName = x.name,
                        hour = hours,
                        product = x.product,
                        quantity = x.speed
                    });
 //                   .AsParallel();
 //                   .ToList();
                    
                    
                    //from f in factories
                    //       select new WarehouseProductLog
                    //       {
                    //           factoryName = f.name,
                    //           quantity = f.speed,
                    //           hour = hours,
                    //           product = f.product
                    //       };

                foreach (var s in temp)
                {
                    Console.WriteLine(s.factoryName);
                }


                hours++;
            }
        }
        

        

    }
}
