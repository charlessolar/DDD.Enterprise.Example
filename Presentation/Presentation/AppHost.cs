using Demo.Infrastructure.Library.SSE;
using Demo.Library.IoC;
using Demo.Library.Security;
using Demo.Library.Validation;
using Demo.Presentation.Inventory.Items;
using NServiceBus;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Caching;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Logging.Log4Net;
using ServiceStack.Messaging;
using ServiceStack.Razor;
using ServiceStack.Redis;
using ServiceStack.Text;
using ServiceStack.Validation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Demo.Presentation
{
    public class AppHost : AppHostBase, IConfigureThisEndpoint
    {
        //Tell Service Stack the name of your application and where to find your web services
        public AppHost()
            : base("Demo Web Services", typeof(AppHost).Assembly)
        {
        }

        public override ServiceStackHost Init()
        {
            LogManager.LogFactory = new Log4NetFactory(true);
            log4net.Config.XmlConfigurator.Configure();


            var serverEvents = new MemoryServerEvents
            {
                Timeout = TimeSpan.FromSeconds(30),
                NotifyChannelOfSubscriptions = false
            };

            serverEvents.OnSubscribe = (x) =>
                {
                    Console.Write(x.DisplayName);
                };

            ObjectFactory.Initialize(x =>
            {
                x.For<IManager>().Use<Manager>();
                x.For<ICacheClient>().Use(new MemoryCacheClient());
                x.For<IServerEvents>().Use(serverEvents);
                x.For<ISubscriptionManager>().Use(new MemorySubscriptionManager());
            });


            // Comment out if you lack a NServiceBus license (trial required)
            NServiceBus.Configure.Instance.LicensePath(@"C:\License.xml");

            NServiceBus.Configure.Transactions.Advanced(t => t.DefaultTimeout(new TimeSpan(0, 5, 0)));
            NServiceBus.Configure.Serialization.Json();
            var bus = NServiceBus.Configure
                .With(AllAssemblies.Matching("Presentation").And("Application").And("Domain").And("Library"))
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

            return base.Init();
        }

        public override void Configure(Funq.Container container)
        {
            JsConfig.IncludeNullValues = true;
            JsConfig.AlwaysUseUtc = true;

            container.Adapter = new StructureMapContainerAdapter();

            Plugins.Add(new SessionFeature());
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new RazorFormat());
            Plugins.Add(new RequestLogsFeature());

            Plugins.Add(new PostmanFeature());
            Plugins.Add(new CorsFeature());

            Plugins.Add(new ValidationFeature());
            Plugins.Add(new ServerEventsFeature());
            Plugins.Add(new Presentation.Inventory.Plugin());


            //container.Register<IRedisClientsManager>(c =>
            //    new PooledRedisClientManager("localhost:6379"));
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetClient());

            //container.Register<ICacheClient>(new MemoryCacheClient());


            //container.Register<IBus>(bus);
        }
    }
}