using NServiceBus;
using Presentation.Inventory.Items;
using Raven.Client;
using Raven.Client.Document;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    public class AppHost : AppHostBase
    {
        //Tell Service Stack the name of your application and where to find your web services
        public AppHost()
            : base("Demo Web Services", typeof(Items).Assembly)
        {
        }

        public override void Configure(Funq.Container container)
        {
            var store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "Demo-ReadModels" };
            store.Initialize();


            NServiceBus.Configure.Transactions.Advanced(t => t.DefaultTimeout(new TimeSpan(0, 5, 0)));
            NServiceBus.Configure.Serialization.Json();
            var bus = NServiceBus.Configure.With(typeof(Domain.Inventory.Items.Commands.Create).Assembly)
                .DefaultBuilder()
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
                .UnicastBus()
                .RavenPersistence()
                .RavenSubscriptionStorage()
                .UseInMemoryTimeoutPersister()
                .InMemoryFaultManagement()
                .InMemorySagaPersister()
                .SendOnly();


            container.Register<IDocumentStore>(store);
            container.Register<IBus>(bus);
        }
    }
}