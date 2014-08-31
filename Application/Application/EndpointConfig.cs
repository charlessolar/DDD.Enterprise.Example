namespace Demo.Application
{
    using Demo.Library.Security;
    using Demo.Library.Validation;
    using FluentValidation;
    using log4net;
    using NServiceBus;
    using Raven.Client;
    using Raven.Client.Document;
    using StructureMap;
    using System;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization, ISpecifyMessageHandlerOrdering
    {
        public void Init()
        {
            log4net.Config.XmlConfigurator.Configure();

            ObjectFactory.Initialize(x =>
            {
                x.For<IManager>().Use<Manager>();
            });

            // Comment out if you lack a NServiceBus license (trial required)
            Configure.Instance.LicensePath(@"C:\License.xml");

            Configure.Transactions.Advanced(t => t.DefaultTimeout(new TimeSpan(0, 5, 0)));
            Configure.Serialization.Json();
            Configure
                .With(AllAssemblies.Matching("Application").And("Domain").And("Library"))
                .DefineEndpointName("Application")
                .StructureMapBuilder()
                .Log4Net()
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Events"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Commands"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && (t.Namespace.EndsWith("Messages") || t.Namespace.EndsWith("Queries")))
                .UnicastBus()
                .InMemorySubscriptionStorage()
                .UseInMemoryTimeoutPersister()
                .InMemoryFaultManagement()
                .InMemorySagaPersister();

            var store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "Demo-ReadModels" };
            store.Initialize();

            Configure.Instance.Configurer.RegisterSingleton<IDocumentStore>(store);
            Configure.Instance.Configurer.RegisterSingleton<IValidatorFactory>(ObjectFactory.GetInstance<StructureMapValidatorFactory>());
        }
        public void SpecifyOrder(Order order)
        {
            order.Specify(First<ValidationMessageHandler>.Then<SecurityMessageHandler>());
        }
    }
}