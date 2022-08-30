using WarehouseApp;

Warehouse t = new Warehouse();

t.InitStatsOutput();

await t.Work();

Console.WriteLine("------------------------------------");
t.InLogOutput();

Console.WriteLine("------------------------------------");
t.TruckStat();