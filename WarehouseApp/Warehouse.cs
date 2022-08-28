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

        double toDelivery; //95% наполняемости склада

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
                Factory f = new Factory(alphabet.Substring(i % 26, 1), koef * n);

                factories.Add(f);
                koef += 0.1;
            }

            perHour = (from f in factories.AsParallel()
                       select f).Sum(f => f.speed);

            warehouseCapacity = M * perHour;

            toDelivery = warehouseCapacity * 0.95;

        }

        internal async Task Work()
        {
            int hours = 150; //кол-во часов работы

            IEnumerable<WarehouseIncomingProductLog> inLog = new List<WarehouseIncomingProductLog>();
            IEnumerable<WarehouseIncomingProductLog> inLog2 = new List<WarehouseIncomingProductLog>();
            List<WarehouseLeavingProductLog> truckLog = new List<WarehouseLeavingProductLog>();

            for (int i = 1; i <= hours; i++)
            {
                Console.WriteLine("Время: " + i);

                int currentHour = i;

                var temp = factories.AsParallel()
                    .Select(x => new WarehouseIncomingProductLog
                    {
                        factoryName = x.name,
                        hourIn = currentHour,
                        product = x.product,
                        quantity = x.speed
                    });

                inLog = inLog.Concat(temp);
                inLog2 = inLog2.Concat(temp);

                if ((inLog.Sum(x => x.quantity) + perHour) >= toDelivery)
                {
                    foreach (var t in trucks)
                    {
                        double load = 0;

                        foreach (var l in inLog2)
                        {
                            if (load + l.quantity <= t.capacity && !l.factoryName.Contains("_Leave"))
                            {
                                load += l.quantity;
                                truckLog.Add(new WarehouseLeavingProductLog
                                {
                                    recordIn = l,
                                    truckName = t.name,
                                    hourOut = currentHour
                                });
                                l.factoryName = l.factoryName + "_Leave";
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            

            int j = 1;
            foreach (var t in inLog)
            {
                Console.WriteLine(j + " " + t.factoryName + " " + t.hourIn);
                j++;
            }
        }

        internal IEnumerable<WarehouseLeavingProductLog> PickUp()
        {
            IEnumerable<WarehouseLeavingProductLog> tLog = new List<WarehouseLeavingProductLog>();

            foreach (var t in trucks)
            {

            }

            return tLog;
        }




    }
}
