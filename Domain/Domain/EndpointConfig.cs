namespace Domain
{
    using log4net;
    using NES.NEventStore;
    using NES.NEventStore.Raven;
    using NES.NServiceBus;
    using NEventStore;
    using NServiceBus;
    using NServiceBus.Features;
    using System;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization, IWantToRunWhenBusStartsAndStops
    {
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
                .InMemorySagaPersister()
                .NES();
            //LogManager.GetRepository().Threshold = log4net.Core.Level.Warn;
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
    }
}