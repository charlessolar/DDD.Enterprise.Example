namespace DemoMessages
{
    using log4net;
    using NServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantToRunWhenBusStartsAndStops, IWantCustomInitialization
    {
        public IBus Bus { get; set; }
        public void Init()
        {
            Configure.Transactions.Advanced(t => t.DefaultTimeout(new TimeSpan(0, 5, 0)));
            Configure.Serialization.Json();
            Configure.With()
                .DefaultBuilder()
                .Log4Net()
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
                .UnicastBus()
                .RavenPersistence()
                .RavenSubscriptionStorage()
                .UseInMemoryTimeoutPersister()
                .InMemoryFaultManagement()
                .InMemorySagaPersister();
            LogManager.GetRepository().Threshold = log4net.Core.Level.Warn;
        }

        public void Start()
        {
            Console.WriteLine("Press 'Enter' to send a message. To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                var timings = new List<long>();
                var watch = new Stopwatch();

                watch.Start();
                // Creates a new item
                var newItem = new Domain.Inventory.Items.Commands.Create()
                {
                    ItemId = Guid.NewGuid(),
                    Number = "T1000",
                    Description = "The infamous T1000",
                    UnitOfMeasure = "EA",
                    CatalogPrice = 10.0M,
                    CostPrice = 9.5M
                };
                Bus.Send("domain", newItem);
                watch.Stop();
                timings.Add(watch.ElapsedMilliseconds);
                watch.Reset();

                // Add some stock to said item
                var newStock = new Domain.Inventory.SerialNumbers.Commands.Create()
                {
                    SerialNumberId = Guid.NewGuid(),
                    SerialNumber = "0001",
                    Effective = DateTime.UtcNow,
                    Quantity = 10.0M,
                    ItemId = newItem.ItemId
                };
                Bus.Send("domain", newStock);
                watch.Stop();
                timings.Add(watch.ElapsedMilliseconds);
                watch.Reset();


                watch.Start();
                // Take some stock from item
                Bus.Send("domain", new Domain.Inventory.SerialNumbers.Commands.TakeQuantity()
                {
                    SerialNumberId = newStock.SerialNumberId,
                    Quantity = 2.2M,
                });
                watch.Stop();
                timings.Add(watch.ElapsedMilliseconds);
                watch.Reset();

                watch.Start();
                Bus.Send("domain", new Domain.Inventory.SerialNumbers.Commands.TakeQuantity()
                {
                    SerialNumberId = newStock.SerialNumberId,
                    Quantity = 0.8M,
                });
                watch.Stop();
                timings.Add(watch.ElapsedMilliseconds);
                watch.Reset();

                watch.Start();
                Bus.Send("domain", new Domain.Inventory.SerialNumbers.Commands.TakeQuantity()
                {
                    SerialNumberId = newStock.SerialNumberId,
                    Quantity = 3M,
                });
                watch.Stop();
                timings.Add(watch.ElapsedMilliseconds);
                watch.Reset();

                watch.Start();
                // Change Item's Description
                Bus.Send("domain", new Domain.Inventory.Items.Commands.ChangeDescription()
                {
                    ItemId = newItem.ItemId,
                    Description = "Event sourcing is fun!"
                });
                watch.Stop();
                timings.Add(watch.ElapsedMilliseconds);
                watch.Reset();

                Console.WriteLine("Elapsed times: {0}", String.Join(",", timings.ToArray()));
                Console.WriteLine("--------");
                Console.WriteLine("Finish sending commands, press 'Enter' to send some more. To exit, Ctrl + C");
            }
        }
        public void Stop()
        {
        }


        private static void Main(string[] args)
        {
        }
    }
}