using Aggregates;
using Metrics;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Newtonsoft.Json;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using Demo.Library.Future;
using Demo.Library.GES;
using Demo.Library.IoC;
using Demo.Library.Logging;
using Demo.Library.Security;
using Demo.Library.Setup;
using Demo.Library.Setup.Attributes;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.Text;
using ServiceStack.Validation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Q = Demo.Library.Queries;
using ServiceStack.Redis;
using System.Runtime.ExceptionServices;
using System.IO;
using ServiceStack.OrmLite;
using ServiceStack.Auth;
using Demo.Presentation.ServiceStack.Authentication;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;

namespace Demo.Presentation.ServiceStack
{
    internal class AppHost : AppSelfHostBase
    {
        private IContainer _container;
        private IEnumerable<SetupInfo> _operations;
        private static NLog.ILogger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static Meter _exceptionsMeter = Metric.Meter("Exceptions Reported", Unit.Items);

        //Tell Service Stack the name of your application and where to find your web services
        public AppHost()
            : base("Demo.Api", typeof(AppHost).Assembly)
        {
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.ExceptionObject.ToString());
        }
        private static void ExceptionTrapper(object sender, FirstChanceExceptionEventArgs e)
        {
            _exceptionsMeter.Mark();
            //Logger.DebugFormat("Thrown exception: {0}", e.Exception);
        }
        

        public override ServiceStackHost Init()
        {
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.DefaultConnectionLimit = 1000;

            var conf = NLog.Config.ConfigurationItemFactory.Default;
            conf.LayoutRenderers.RegisterDefinition("logsdir", typeof(LogsDir));
            conf.LayoutRenderers.RegisterDefinition("Domain", typeof(Library.Logging.Domain));
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration($"{AppDomain.CurrentDomain.BaseDirectory}/logging.config");

            LogManager.LogFactory = new global::ServiceStack.Logging.NLogger.NLogFactory();
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            AppDomain.CurrentDomain.FirstChanceException += ExceptionTrapper;
            NServiceBus.Logging.LogManager.Use<NServiceBus.NLogFactory>();
           

            IRedisClientsManager redisClient = null;
            ICacheClient cacheClient;
            IServerEvents serverEvents;
            var connectionString = ConfigurationManager.ConnectionStrings["Redis"];
            if (connectionString == null)
            {
                cacheClient = new MemoryCacheClient();
                serverEvents = new MemoryServerEvents
                {
                    IdleTimeout = TimeSpan.FromSeconds(30),
                    NotifyChannelOfSubscriptions = false
                };
            }
            else
            {
                redisClient = new BasicRedisClientManager(connectionString.ConnectionString);
                cacheClient = new RedisClientManagerCacheClient(redisClient);
                serverEvents = new RedisServerEvents(redisClient)
                {
                    Timeout = TimeSpan.FromSeconds(30),
                    NotifyChannelOfSubscriptions = false,
                };
            }

            IDbConnectionFactory sql = null;
            var connectionStringSQL = ConfigurationManager.ConnectionStrings["SQL"];
            if (connectionStringSQL != null)
            {
                sql = new OrmLiteConnectionFactory(connectionStringSQL.ConnectionString, SqlServer2012Dialect.Provider)
                {
                    ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
                };
            }
            


            _container = new Container(x =>
            {
                if (redisClient != null)
                    x.For<IRedisClientsManager>().Use(redisClient);
                if (sql != null)
                {
                    x.For<IDbConnectionFactory>().Use(sql);
                    x.For<ISubscriptionStorage>().Use<MSSQLStorage>();
                }
                else
                    x.For<ISubscriptionStorage>().Use<MemoryStorage>();

                x.For<IManager>().Use<Manager>();
                x.For<IFuture>().Use<Future>().Singleton();
                x.For<ICacheClient>().Use(cacheClient);
                x.For<IServerEvents>().Use(serverEvents);
                x.For<ISubscriptionManager>().Use<SubscriptionManager>().Singleton();

                x.Scan(y =>
                {
                    AllAssemblies.Matching("Presentation.ServiceStack").ToList().ForEach(a => y.Assembly(a));

                    y.WithDefaultConventions();
                    y.AddAllTypesOf<ISetup>();
                });
            });

            // Do this before bus starts
            InitiateSetup();
            SetupApplication();

            var config = new BusConfiguration();

            Logger.Info("Initializing Service Bus");
            config.LicensePath(ConfigurationManager.AppSettings["license"]);

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            if (string.IsNullOrEmpty(endpoint))
                endpoint = "application.servicestack";

            config.EndpointName(endpoint);

            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));

            config.DisableFeature<Sagas>();
            config.DisableFeature<SecondLevelRetries>();
            // Important to not subscribe to messages as we receive them from the event store
            config.DisableFeature<AutoSubscribe>();

            config.UseTransport<RabbitMQTransport>()
                .CallbackReceiverMaxConcurrency(36)
                .UseDirectRoutingTopology()
                .ConnectionStringName("RabbitMq");

            config.Transactions().DisableDistributedTransactions();
            //config.DisableDurableMessages();

            config.UseSerialization<NewtonsoftSerializer>();

            config.EnableFeature<Aggregates.Feature>();



            if (Logger.IsDebugEnabled)
                config.Pipeline.Register<LogIncomingRegistration>();

            var bus = Bus.Create(config).Start();

            _container.Configure(x => x.For<IBus>().Use(bus).Singleton());

            return base.Init();
        }

        public override void Configure(Funq.Container container)
        {
            JsConfig.IncludeNullValues = true;
            JsConfig.AlwaysUseUtc = true;
            JsConfig.AssumeUtc = true;
            JsConfig.TreatEnumAsInteger = true;
            JsConfig.DateHandler = DateHandler.ISO8601;

            SetConfig(new HostConfig { DebugMode = true, ApiVersion = "1" });

            container.Adapter = new StructureMapContainerAdapter(_container);
            
            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
              new IAuthProvider[] {
                new JwtAuthProvider(AppSettings){ RequireSecureConnection=false },
                new BasicAuthProvider(), //Sign-in with HTTP Basic Auth
                new CredentialsAuthProvider(), //HTML Form post of UserName/Password credentials
              }));
            Plugins.Add(new RegistrationFeature());
            container.Register<IUserAuthRepository>(x => new DomainAuthRepository(x.Resolve<IBus>()));

            Plugins.Add(new SessionFeature());
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new NativeTypesFeature());

            Plugins.Add(new PostmanFeature
            {
                DefaultLabelFmt = new List<String> { "type: english", " ", "route" }
            });
            Plugins.Add(new CorsFeature(
                allowOriginWhitelist: new[] {
                    "http://localhost:8080",
                    "http://localhost:9000",
                    "http://Demo.development.syndeonetwork.com",
                    "http://Demo.syndeonetwork.com"
                },
                allowCredentials: true,
                allowedHeaders: "Content-Type, Authorization",
                allowedMethods: "GET, POST, PUT, DELETE, OPTIONS"
                ));
            

            Plugins.Add(new ValidationFeature());
            Plugins.Add(new Profiling.MetricsFeature());
            Plugins.Add(new ServerEventsFeature());
            Plugins.Add(new ServiceStack.Authentication.Plugin());

            var nativeTypes = this.GetPlugin<NativeTypesFeature>();
            nativeTypes.MetadataTypesConfig.GlobalNamespace = "Demo.DTOs";

            //container.Register<IRedisClientsManager>(c =>
            //    new PooledRedisClientManager("localhost:6379"));
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetClient());

            //container.Register<ICacheClient>(new MemoryCacheClient());

            //container.Register<IBus>(bus);

        }

        private void InitiateSetup()
        {
            _operations = _container.GetAllInstances<ISetup>().Select(o =>
            {
                var operation = o.GetType().GetCustomAttributes(typeof(OperationAttribute), true).FirstOrDefault() as OperationAttribute;
                var depends = o.GetType().GetCustomAttributes(typeof(DependsAttribute), true).FirstOrDefault() as DependsAttribute;
                var category = o.GetType().GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault() as CategoryAttribute;
                return new SetupInfo
                {
                    Name = operation.Name,
                    Depends = depends == null ? new string[] { } : depends.Depends,
                    Category = category.Name,
                    Operation = o
                };
            });
        }

        private void SetupApplication(SetupInfo info = null)
        {
            var watch = new Stopwatch();

            // Depends will be either, ALL setup operations if info is null, or all the operations info depends on
            var depends = _operations;
            if (info != null)
                depends = depends.Where(x => info.Depends.Any() && info.Depends.Contains(x.Name));

            foreach (var depend in depends)
                SetupApplication(depend);

            if (info == null || info.Operation.Done)
                return;

            Logger.Info("**************************************************************");
            Logger.Info("   Running setup operation {0}.{1}".Fmt(info.Category, info.Name));
            Logger.Info("**************************************************************");

            watch.Start();
            if (!info.Operation.Initialize())
            {
                Logger.Info("ERROR - Failed to complete setup operation!");
                Environment.Exit(1);
            }
            watch.Stop();
            Logger.Info("**************************************************************");
            Logger.Info("   Finished operation {0}.{1} in {2}!".Fmt(info.Category, info.Name, watch.Elapsed));
            Logger.Info("**************************************************************");
        }
    }

    internal class SetupInfo
    {
        public String Name { get; set; }

        public String[] Depends { get; set; }

        public String Category { get; set; }

        public ISetup Operation { get; set; }
    }
}