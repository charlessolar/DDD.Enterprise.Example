using Demo.Library.IoC;
using Demo.Library.Validation;
using Demo.Presentation.Inventory.Items;
using NServiceBus;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Logging.Log4Net;
using ServiceStack.Messaging;
using ServiceStack.Razor;
using ServiceStack.Redis;
using ServiceStack.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Demo.Presentation
{
    public class AppHost : AppHostBase
    {
        //Tell Service Stack the name of your application and where to find your web services
        public AppHost()
            : base("Demo Web Services", typeof(AppHost).Assembly)
        {
        }

        public override void Configure(Funq.Container container)
        {
            Plugins.Add(new RazorFormat());
            //Plugins.Add(new ValidationFeature());
            Plugins.Add(new Presentation.Inventory.Plugin());

            container.RegisterValidators(typeof(Presentation.Inventory.Plugin).Assembly);

            container.Adapter = new StructureMapContainerAdapter();

            //container.Register<IRedisClientsManager>(c =>
            //    new PooledRedisClientManager("localhost:6379"));
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetClient());

            container.Register<ICacheClient>(new MemoryCacheClient());

            // Comment out if you lack a NServiceBus license (trial required)
            NServiceBus.Configure.Instance.LicensePath(@"C:\License.xml");

            NServiceBus.Configure.Transactions.Advanced(t => t.DefaultTimeout(new TimeSpan(0, 5, 0)));
            NServiceBus.Configure.Serialization.Json();
            var bus = NServiceBus.Configure
                .With(AllAssemblies.Except("ServiceStack"))
                .DefineEndpointName("Presentation")
                .StructureMapBuilder()
                .Log4Net()
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Events"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Commands"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && (t.Namespace.EndsWith("Messages") || t.Namespace.EndsWith("Queries")))
                .UnicastBus()
                .InMemorySubscriptionStorage()
                .UseInMemoryTimeoutPersister()
                .InMemoryFaultManagement()
                .InMemorySagaPersister()
                .CreateBus()
                .Start();

            LogManager.LogFactory = new Log4NetFactory(true);
            log4net.Config.XmlConfigurator.Configure();

            container.Register<IBus>(bus);
        }
    }
}