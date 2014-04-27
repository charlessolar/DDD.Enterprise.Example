using NServiceBus;
using Presentation.Inventory.Items;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Messaging;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            container.Register<IRedisClientsManager>(c =>
                new PooledRedisClientManager("localhost:6379"));
            container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
            container.Register(c => c.Resolve<IRedisClientsManager>().GetClient());


            NServiceBus.Configure.Transactions.Advanced(t => t.DefaultTimeout(new TimeSpan(0, 5, 0)));
            NServiceBus.Configure.Serialization.Json();
            var bus = NServiceBus.Configure.With(AllAssemblies.Except("ServiceStack"))
                .DefaultBuilder()
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
                .DefiningCommandsAs(t => t.Namespace != null && (t.Namespace.EndsWith("Commands") || t.Namespace.EndsWith("Queries")))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"))
                .UnicastBus()
                .RavenPersistence()
                .RavenSubscriptionStorage()
                .UseInMemoryTimeoutPersister()
                .InMemoryFaultManagement()
                .InMemorySagaPersister()
                .CreateBus()
                .Start();

            container.Register<IBus>(bus);

            NServiceBus.Configure.Component<IRedisClient>(c => container.Resolve<IRedisClientsManager>().GetClient(), DependencyLifecycle.SingleInstance);
        }
    }
}