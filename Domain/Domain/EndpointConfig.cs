namespace Domain
{
    using FluentValidation;
    using Library.Authorization;
    using Library.Validation;
    using log4net;
    using NES;
    using NES.NEventStore;
    using NES.NEventStore.Raven;
    using NES.NServiceBus;
    using NEventStore;
    using NServiceBus;
    using NServiceBus.Features;
    using StructureMap;
    using System;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization, IWantToRunWhenBusStartsAndStops, ISpecifyMessageHandlerOrdering
    {
        public void Init()
        {
            ObjectFactory.Initialize(x =>
                {
                    x.For<IRepository>().Use<Repository>();
                });

            Configure.Transactions.Advanced(t => t.DefaultTimeout(new TimeSpan(0, 5, 0)));
            Configure.Serialization.Json();
            Configure
                .With(AllAssemblies.Except("ServiceStack"))
                .StructureMapBuilder()
                .Log4Net()
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
                .DefiningCommandsAs(t => t.Namespace != null && (t.Namespace.EndsWith("Commands") || t.Namespace.EndsWith("Queries")))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"))
                .UnicastBus()
                .RavenPersistence()
                .RavenSubscriptionStorage()
                .UseInMemoryTimeoutPersister()
                .InMemoryFaultManagement()
                .InMemorySagaPersister()
                .NES();

            LogManager.GetRepository().Threshold = log4net.Core.Level.Warn;
            Configure.Instance.Configurer.RegisterSingleton<IValidatorFactory>(new StructureMapValidatorFactory());
        }
        public void Start()
        {
            Wireup.Init()
                      .UsingRavenPersistence("Demo")
                      .ConsistentQueries()
                      .InitializeStorageEngine()
                      .NES()
                      .UsingJsonSerialization()
                      .Build();
            //.StartDispatchScheduler();
        }

        public void Stop()
        {
        }
        public void SpecifyOrder(Order order)
        {
            order.Specify(First<ValidationMessageHandler>.Then<AuthorizationMessageHandler>());
        }
    }
}