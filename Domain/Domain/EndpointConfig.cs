namespace Demo.Domain
{
    using Aggregates;
    using Demo.Domain.Security;
    using Demo.Library.Command;
    using Demo.Library.Security;
    using Demo.Library.Validation;
    using NEventStore;
    using NServiceBus;
    using NServiceBus.Log4Net;
    using StructureMap;
    using System.Configuration;


    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */

    public class EndpointConfig : IConfigureThisEndpoint, ISpecifyMessageHandlerOrdering
    {
        private IContainer _container;

        public void Customize(BusConfiguration config)
        {
            log4net.Config.XmlConfigurator.Configure();
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            _container = new Container(x =>
            {
                x.For<IManager>().Use<Manager>();
            });

            //var conventions = config.Conventions();
            //conventions
            //    .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Events"))
            //    .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Commands"))
            //    .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && (t.Namespace.EndsWith("Messages") || t.Namespace.EndsWith("Queries")));

            config.LicensePath(@"C:\License.xml");

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            if (string.IsNullOrEmpty(endpoint))
                endpoint = "domain";

            config.EndpointName(endpoint);

            //config.AssembliesToScan(AllAssemblies.Matching("Domain").And("Library"));

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));
            config.UseSerialization<NServiceBus.JsonSerializer>();

            config.EnableInstallers();

            config.UseAggregates(y =>
            {
                return Wireup.Init()
                        .UseAggregates(y)
                        //.UsingRavenPersistence("Demo")
                        .UsingInMemoryPersistence()
                        .InitializeStorageEngine()
                        .Build();
            },
            () => new Accept(),
            (message) => new Reject { Message = message }
            );
        }

        public void SpecifyOrder(Order order)
        {
            order.Specify(First<ValidationMessageHandler>.Then<SecurityMessageHandler>());
        }
    }
}