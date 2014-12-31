namespace Demo.Application
{
    using Demo.Library.Security;
    using Demo.Library.Validation;
    using FluentValidation;
    using log4net;
    using NServiceBus;
    using NServiceBus.Log4Net;
    using Raven.Client;
    using Raven.Client.Document;
    using StructureMap;
    using System;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, ISpecifyMessageHandlerOrdering
    {
        private IContainer _container;

        public void Customize(BusConfiguration config)
        {
            log4net.Config.XmlConfigurator.Configure();
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();


            var store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "Demo-ReadModels" }.Initialize();

            _container = new Container(x =>
            {
                x.For<IManager>().Use<Manager>();
                x.For<IDocumentStore>().Use(store).Singleton();
            });


            var conventions = config.Conventions();
            conventions
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Events"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Commands"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && (t.Namespace.EndsWith("Messages") || t.Namespace.EndsWith("Queries")));

            config.LicensePath(@"C:\License.xml");

            config.EndpointName("Application");
            config.EndpointVersion("0.0.0");
            config.AssembliesToScan(AllAssemblies.Matching("Application").And("Domain").And("Library"));

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));
            config.UseSerialization<NServiceBus.JsonSerializer>();


        }
        public void SpecifyOrder(Order order)
        {
            order.Specify(First<ValidationMessageHandler>.Then<SecurityMessageHandler>());
        }
    }
}