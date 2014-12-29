namespace Demo.DemoMessages
{
    using log4net;
    using NServiceBus;
    using NServiceBus.Log4Net;
    using StructureMap;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantToRunWhenBusStartsAndStops
    {
        private IContainer _container;
        public IBus Bus { get; set; }

        public void Customize(BusConfiguration config)
        {
            log4net.Config.XmlConfigurator.Configure();
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();


            var conventions = config.Conventions();
            conventions
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Events"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Commands"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && (t.Namespace.EndsWith("Messages") || t.Namespace.EndsWith("Queries")));

            config.LicensePath(@"C:\License.xml");

            config.EndpointName("DemoMessages");
            config.EndpointVersion("0.0.0");
            config.AssembliesToScan(AllAssemblies.Matching("DemoMessages").And("Domain"));

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));
            config.UseSerialization<NServiceBus.JsonSerializer>();
            

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