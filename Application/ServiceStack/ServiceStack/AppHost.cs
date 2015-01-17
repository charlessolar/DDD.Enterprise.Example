using Demo.Infrastructure.Library.SSE;
using Demo.Library.Authentication;
using Demo.Library.Extensions;
using Demo.Library.IoC;
using Demo.Library.Security;
using NServiceBus;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Logging.Log4Net;
using ServiceStack.Text;
using ServiceStack.Validation;
using ServiceStack.Web;
using StructureMap;
using System;

namespace Demo.Application.ServiceStack
{
    public class AppHost : AppHostBase
    {
        private IContainer _container;

        //Tell Service Stack the name of your application and where to find your web services
        public AppHost()
            : base("Demo.Api", typeof(AppHost).Assembly)
        {
        }

        public override ServiceStackHost Init()
        {
            log4net.Config.XmlConfigurator.Configure();
            LogManager.LogFactory = new Log4NetFactory();
            NServiceBus.Logging.LogManager.Use<NServiceBus.Log4Net.Log4NetFactory>();

            var serverEvents = new MemoryServerEvents
            {
                IdleTimeout = TimeSpan.FromSeconds(30),
                NotifyChannelOfSubscriptions = false
            };

            _container = new Container(x =>
            {
                x.For<IManager>().Use<Manager>();
                x.For<ICacheClient>().Use(new MemoryCacheClient());
                x.For<IServerEvents>().Use(serverEvents);
                x.For<ISubscriptionManager>().Use<MemorySubscriptionManager>();
            });

            var config = new BusConfiguration();
            //var conventions = config.Conventions();
            //conventions
            //    .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Events"))
            //    .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Commands"))
            //    .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && (t.Namespace.EndsWith("Messages") || t.Namespace.EndsWith("Queries")));

            config.LicensePath(@"C:\License.xml");

            config.EndpointName("Presentation");
            config.EndpointVersion("0.0.0");
            //config.AssembliesToScan(AllAssemblies.Matching("Presentation").And("Application").And("Domain").And("Library"));

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));
            config.UseSerialization<NServiceBus.JsonSerializer>();

            var bus = Bus.Create(config).Start();

            _container.Configure(x => x.For<IBus>().Use(bus).Singleton());

            return base.Init();
        }

        public override void Configure(Funq.Container container)
        {
            JsConfig.IncludeNullValues = true;
            JsConfig.AlwaysUseUtc = true;

            SetConfig(new HostConfig { DebugMode = true });

            container.Adapter = new StructureMapContainerAdapter(_container);

            Plugins.Add(new SessionFeature());
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new RequestLogsFeature());

            Plugins.Add(new PostmanFeature());
            Plugins.Add(new CorsFeature(allowedHeaders: "Content-Type, Authorization"));

            var appSettings = new AppSettings();

            Plugins.Add(new ValidationFeature());
            Plugins.Add(new ServerEventsFeature());
            Plugins.Add(new ServiceStack.Inventory.Plugin());
            Plugins.Add(new ServiceStack.Authentication.Plugin());

            //container.Register<IRedisClientsManager>(c =>
            //    new PooledRedisClientManager("localhost:6379"));
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetClient());

            //container.Register<ICacheClient>(new MemoryCacheClient());

            //container.Register<IBus>(bus);
        }
    }
}
