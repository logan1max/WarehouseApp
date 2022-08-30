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
        private int M;
        private int n;
        private int k; //кол-во заводов

        private double perHour; //кол-во продукции в час

        private double warehouseCapacity; //вместимость склада

        private double toDelivery; //95% наполняемости склада

        private List<Factory> factories; //список заводов
        private List<Truck> trucks; //список грузовиков

        private List<WarehouseIncomingProductLog> inLog; //журнал приходящей продукции
        private List<WarehouseLeavingProductLog> truckLog; //журнал отгрузок 

        internal Warehouse()
        {
            M = 100;
            n = 300;
            k = 10;

            factories = new List<Factory>();
            trucks = new List<Truck>() 
            { 
                new Truck("First", 3000), 
                new Truck("Second", 5000),
                new Truck("Third", 9000),
            };

            inLog = new List<WarehouseIncomingProductLog>();
            truckLog = new List<WarehouseLeavingProductLog>();            

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

        /// <summary>
        /// Вывод начальных значений
        /// </summary>
        internal void InitStatsOutput()
        {
            Console.WriteLine("M: {0} | n: {1}", M, n);
            Console.WriteLine("Number of factories: {0}", k);
            Console.WriteLine("Number of production per hour: {0:f2}", perHour);
            Console.WriteLine("Warehouse capacity: {0:f2}", warehouseCapacity);
            Console.WriteLine("95% of warehouse capacity: {0:f2}", toDelivery);

            foreach(var f in factories)
            {
                Console.WriteLine("Factory Name: {0} | Num per Hour : {1:f2} | Product Name: {2} | Product weight: {3:f2} | Package type: {4}", f.name, f.speed, f.product.name, f.product.weight, f.product.packageType);
            }
        }


        /// <summary>
        /// Метод описывающий работу склада
        /// </summary>
        /// <returns></returns>
        internal async Task Work()
        {
            int hours = 200; //кол-во часов работы

            for (int i = 1; i <= hours; i++)
            {
                int currentHour = i;

                var temp = factories.AsParallel()
                    .Select(x => new WarehouseIncomingProductLog
                    {
                        factoryName = x.name,
                        hourIn = currentHour,
                        product = x.product,
                        quantity = x.speed
                    });

                inLog = inLog.Concat(temp).ToList();

                double currentSum = inLog.Sum(x => x.quantity);

                if (currentSum >= toDelivery)
                {
                    await PickUp(currentHour);
                }
            }
        }


        /// <summary>
        /// Вывод на экран журнала приходящей продукции
        /// </summary>
        internal void InLogOutput()
        {
            Console.WriteLine("Incoming production log...");
            Console.WriteLine("------------------------------------");
            int k = 1;
            foreach (var t in inLog)
            {
                Console.WriteLine("N: {0}| Factory: {1} | Hour: {2} | Quantity: {3:f2} | isLeave: {4}", k, t.factoryName, t.hourIn, t.quantity, t.isLeave);
                k++;
            }
        }


        /// <summary>
        /// Отгрузка со склада при наполняемости 95%
        /// </summary>
        /// <param name="currentHour"></param>
        /// <returns></returns>
        private Task PickUp(int currentHour)
        {
            foreach (var t in trucks)
            {
                double load = 0;

                foreach (var l in inLog.AsParallel())
                {
                    if ((load + l.quantity) <= t.capacity && !l.isLeave)
                    {
                        load += l.quantity;
                        truckLog.Add(new WarehouseLeavingProductLog
                        {
                            recordIn = l,
                            truckName = t.name,
                            hourOut = currentHour
                        });

                        l.isLeave = true;
                    }
                    else if ((load + l.quantity) >= t.capacity)
                    {
                        break;
                    }
                }

            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Подсчет и вывод статистики по перевозкам грузовиков
        /// </summary>
        internal void TruckStat()
        {
            Console.WriteLine("Leaving production log...");
            Console.WriteLine("------------------------------------");
            foreach (var truck in trucks)
            {
                var stat = from t in truckLog.AsParallel()
                           where t.truckName == truck.name
                           group t by t.recordIn.product.name into g
                           select new { Name = g.Key, Count = g.Count() };

                Console.WriteLine("Truck: " + truck.name);

                foreach(var s in stat)
                {
                    Console.WriteLine("ProductName: " + s.Name + " Count: " + s.Count);
                }
            }
        }
    }
}
