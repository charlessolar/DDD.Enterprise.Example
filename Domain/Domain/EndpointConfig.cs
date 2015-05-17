namespace Demo.Domain
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using Aggregates;
    using Demo.Domain.Security;
    using Demo.Library.Command;
    using Demo.Library.Security;
    using Demo.Library.Validation;
    using EventStore.ClientAPI;
    using EventStore.ClientAPI.Embedded;
    using EventStore.ClientAPI.SystemData;
    using log4net;
    using NServiceBus;
    using NServiceBus.Log4Net;
    using StructureMap;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */

    public class EndpointConfig : IConfigureThisEndpoint, ISpecifyMessageHandlerOrdering
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EndpointConfig));
        private IContainer _container;

        public IEventStoreConnection ConfigureStore()
        {
            var path = ConfigurationManager.AppSettings["eventstorePath"];
            var tcpPort = ConfigurationManager.AppSettings["eventstoreTcp"];
            var httpPort = ConfigurationManager.AppSettings["eventstoreHttp"];

            Int32 tcpExtPort = 1113;
            Int32 httpExtPort = 2112;

            Int32.TryParse(tcpPort, out tcpExtPort);
            Int32.TryParse(httpPort, out httpExtPort);

            System.IO.Directory.CreateDirectory(path);

            var embedded = EmbeddedVNodeBuilder
                            .AsSingleNode()
                            .RunOnDisk(path)
                            .RunProjections(ProjectionsMode.All)
                            .WithInternalTcpOn(new IPEndPoint(IPAddress.Loopback, tcpExtPort - 1))
                            .WithExternalTcpOn(new IPEndPoint(IPAddress.Loopback, tcpExtPort))
                            .WithInternalHttpOn(new IPEndPoint(IPAddress.Loopback, httpExtPort - 1))
                            .WithExternalHttpOn(new IPEndPoint(IPAddress.Loopback, httpExtPort))
                            .Build();

            embedded.NodeStatusChanged += (sender, e) =>
            {
                Logger.InfoFormat("EventStore status changed: {0}", e.NewVNodeState);
            };

            embedded.Start();

            var settings = ConnectionSettings.Create()
                .UseConsoleLogger()
                .KeepReconnecting();

            var client = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, tcpExtPort));

            client.ConnectAsync().Wait();

            return client;
        }

        public void Customize(BusConfiguration config)
        {
            log4net.Config.XmlConfigurator.Configure();
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            var client = ConfigureStore();

            _container = new Container(x =>
            {
                x.For<IManager>().Use<Manager>();
                x.For<IEventStoreConnection>().Use(client).Singleton();
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

            config.EnableFeature<AggregatesNet>();
            config.EnableFeature<EventStore>();
        }

        public void SpecifyOrder(Order order)
        {
            order.Specify(First<ValidationMessageHandler>.Then<SecurityMessageHandler>());
        }
    }
}