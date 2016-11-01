using Aggregates;
using Metrics;
using NServiceBus;
using NServiceBus.Features;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using Demo.Library.Future;
using Demo.Library.IoC;
using Demo.Library.Logging;
using Demo.Library.Demo;
using Demo.Library.Security;
using Demo.Library.Setup;
using Demo.Library.Setup.Attributes;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Caching;
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
using System.Threading;
using Q = Demo.Library.Queries;
using ServiceStack.Redis;
using System.Runtime.ExceptionServices;
using ServiceStack.OrmLite;
using ServiceStack.Auth;
using Demo.Presentation.ServiceStack.Authentication;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using Aggregates.Contracts;
using System.Threading.Tasks;
using Demo.Library.RabbitMq;
using RabbitMQ.Client;

namespace Demo.Presentation.ServiceStack
{
    internal class AppHost : AppSelfHostBase
    {
        private IContainer _container;
        private IEnumerable<SetupInfo> _operations;
        private static readonly NLog.ILogger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly Meter ExceptionsMeter = Metric.Meter("Exceptions Reported", Unit.Items);

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
            ExceptionsMeter.Mark();
            //Logger.DebugFormat("Thrown exception: {0}", e.Exception);
        }

        public IDemo ConfigureDemo()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Demo"];
            var service = new Library.Demo.Demo();


            var data = connectionString.ConnectionString.Split(';');

            var url = data.FirstOrDefault(x => x.StartsWith("Url", StringComparison.CurrentCultureIgnoreCase));
            if (url == null)
                throw new ArgumentException("No URL parameter in pulse connection string");
            Guid stashId;
            var stashIdStr = data.FirstOrDefault(x => x.StartsWith("StashId=", StringComparison.CurrentCultureIgnoreCase));
            if (stashIdStr.IsNullOrEmpty() || !Guid.TryParse(stashIdStr.Substring(8), out stashId))
                throw new ArgumentException("No StashId parameter in pulse connection string");
            var stash = data.FirstOrDefault(x => x.StartsWith("Stash=", StringComparison.CurrentCultureIgnoreCase));
            if (stash.IsNullOrEmpty())
                throw new ArgumentException("No Stash parameter in pulse connection string");
            var secret = data.FirstOrDefault(x => x.StartsWith("Secret=", StringComparison.CurrentCultureIgnoreCase));
            if (secret.IsNullOrEmpty())
                throw new ArgumentException("No Secret parameter in pulse connection string");

            url = url.Substring(4);
            stash = stash.Substring(6);
            secret = secret.Substring(7);

            ThreadPool.QueueUserWorkItem((_) =>
            {
                service.Init(url, stashId, secret, stash, "Servicestack").Wait();
            });

            return service;
        }

        public void ConfigureMetrics()
        {
            Logger.Info("Setting up Metrics");
            var connectionString = ConfigurationManager.ConnectionStrings["Metric"];
            if (connectionString == null)
                throw new ArgumentException("No metrics connection string found");

            var data = connectionString.ConnectionString.Split(';');

            var url = data.FirstOrDefault(x => x.StartsWith("Url", StringComparison.CurrentCultureIgnoreCase));
            if (url == null)
                throw new ArgumentException("No URL parameter in metrics connection string");
            Guid stashId;
            var stashIdStr = data.FirstOrDefault(x => x.StartsWith("StashId=", StringComparison.CurrentCultureIgnoreCase));
            if (stashIdStr.IsNullOrEmpty() || !Guid.TryParse(stashIdStr.Substring(8), out stashId))
                throw new ArgumentException("No StashId parameter in metrics connection string");
            var stash = data.FirstOrDefault(x => x.StartsWith("Stash=", StringComparison.CurrentCultureIgnoreCase));
            if (stash.IsNullOrEmpty())
                throw new ArgumentException("No Stash parameter in metrics connection string");
            var secret = data.FirstOrDefault(x => x.StartsWith("Secret=", StringComparison.CurrentCultureIgnoreCase));
            if (secret.IsNullOrEmpty())
                throw new ArgumentException("No Secret parameter in metrics connection string");
            var @event = data.FirstOrDefault(x => x.StartsWith("Event=", StringComparison.CurrentCultureIgnoreCase));
            if (string.IsNullOrEmpty(@event))
                throw new ArgumentException("No Event parameter in metrics connection string");

            url = url.Substring(4);
            stash = stash.Substring(6);
            secret = secret.Substring(7);
            @event = @event.Substring(6);


            var service = new Library.Demo.Demo();

            ThreadPool.QueueUserWorkItem((_) =>
            {
                service.Init(url, stashId, secret, stash, "Performance Counters").Wait();
            });
            //Metric.Config
            //    .WithAppCounters()
            //    .WithDemo(service, @event, TimeSpan.FromSeconds(30));
        }
        public override ServiceStackHost Init()
        {
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.DefaultConnectionLimit = 1000;

            var conf = NLog.Config.ConfigurationItemFactory.Default;
            conf.LayoutRenderers.RegisterDefinition("logsdir", typeof(LogsDir));
            conf.LayoutRenderers.RegisterDefinition("Instance", typeof(Library.Logging.Instance));
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

            IDbConnectionFactory sql = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var connectionStringSql = ConfigurationManager.ConnectionStrings["SQL"];
            if (connectionStringSql != null)
            {
                sql = new OrmLiteConnectionFactory(connectionStringSql.ConnectionString, SqlServer2012Dialect.Provider)
                {
                    ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
                };
            }

            var pulse = ConfigureDemo();
            var rabbit = ConfigureRabbit();
            ConfigureMetrics();


            _container = new Container(x =>
            {
                if (redisClient != null)
                    x.For<IRedisClientsManager>().Use(redisClient);

                x.For<IDbConnectionFactory>().Use(sql);
                x.For<ISubscriptionStorage>().Use<MssqlStorage>();


                x.For<IManager>().Use<Manager>();
                x.For<IFuture>().Use<Future>().Singleton();
                x.For<ICacheClient>().Use(cacheClient);
                x.For<IServerEvents>().Use(serverEvents);
                x.For<ISubscriptionManager>().Use<SubscriptionManager>().Singleton();
                x.For<IDemo>().Use(pulse);
                x.For<IConnection>().Use(rabbit);


                x.Scan(y =>
                {
                    y.TheCallingAssembly();
                    y.AssembliesFromApplicationBaseDirectory((assembly) => assembly.FullName.StartsWith("Presentation.ServiceStack"));

                    y.WithDefaultConventions();
                    y.AddAllTypesOf<ISetup>();
                    y.AddAllTypesOf<ICommandMutator>();

                    y.ConnectImplementationsToTypesClosing(typeof(Q.IChange<,>));
                });
            });

            // Do this before bus starts
            InitiateSetup();
            SetupApplication().Wait();

            var bus = InitBus().Result;
            _container.Configure(x => x.For<IMessageSession>().Use(bus).Singleton());
            
            return base.Init();
        }

        private async Task<IEndpointInstance> InitBus()
        {
            NServiceBus.Logging.LogManager.Use<NLogFactory>();

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            if (string.IsNullOrEmpty(endpoint))
                endpoint = "application.servicestack";

            var config = new EndpointConfiguration(endpoint);
            config.MakeInstanceUniquelyAddressable(Defaults.Instance.ToString());

            Logger.Info("Initializing Service Bus");
            config.LicensePath(ConfigurationManager.AppSettings["license"]);

            config.EnableInstallers();
            config.LimitMessageProcessingConcurrencyTo(100);
            config.UseTransport<RabbitMQTransport>()
                //.CallbackReceiverMaxConcurrency(4)
                //.UseDirectRoutingTopology()
                .ConnectionStringName("RabbitMq")
                .PrefetchMultiplier(5)
                .UseRoutingTopology<ShardedRoutingTopology>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromSeconds(30));
            
            config.UseSerialization<NewtonsoftSerializer>();

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));


            if (Logger.IsDebugEnabled)
            {
                config.EnableSlowAlerts(true);
                //config.EnableCriticalTimePerformanceCounter();
                config.Pipeline.Register(
                    behavior: typeof(LogIncomingMessageBehavior),
                    description: "Logs incoming messages"
                    );
            }

            config.EnableFeature<Aggregates.Feature>();
            config.Recoverability().ConfigureForAggregates();
            //config.EnableFeature<RoutedFeature>();
            config.DisableFeature<Sagas>();
            

            return await NServiceBus.Endpoint.Start(config).ConfigureAwait(false);
        }
        public static IConnection ConfigureRabbit()
        {

            var connectionString = ConfigurationManager.ConnectionStrings["RabbitMq"];
            if (connectionString == null)
                throw new ArgumentException("No Rabbit connection string");

            var data = connectionString.ConnectionString.Split(';');
            var host = data.FirstOrDefault(x => x.StartsWith("host", StringComparison.CurrentCultureIgnoreCase));
            if (host == null)
                throw new ArgumentException("No HOST parameter in rabbit connection string");
            var virtualhost = data.FirstOrDefault(x => x.StartsWith("virtualhost=", StringComparison.CurrentCultureIgnoreCase));

            var username = data.FirstOrDefault(x => x.StartsWith("username=", StringComparison.CurrentCultureIgnoreCase));
            var password = data.FirstOrDefault(x => x.StartsWith("password=", StringComparison.CurrentCultureIgnoreCase));

            host = host.Substring(5);
            virtualhost = virtualhost?.Substring(12) ?? "/";
            username = username?.Substring(9) ?? "guest";
            password = password?.Substring(9) ?? "guest";

            var factory = new ConnectionFactory {Uri = $"amqp://{username}:{password}@{host}:5672/{virtualhost}"};

            return factory.CreateConnection();
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

            Plugins.Add(new SessionFeature());
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new NativeTypesFeature());

            Plugins.Add(new PostmanFeature
            {
                DefaultLabelFmt = new List<string> { "type: english", " ", "route" }
            });
            Plugins.Add(new CorsFeature(
                allowOriginWhitelist: new[] {
                    "http://localhost:8080",
                    "http://localhost:9000",
                    "http://pulse.development.syndeonetwork.com",
                    "http://pulse.syndeonetwork.com"
                },
                allowCredentials: true,
                allowedHeaders: "Content-Type, Authorization",
                allowedMethods: "GET, POST, PUT, DELETE, OPTIONS"
                ));

            if (Logger.IsDebugEnabled)
                Plugins.Add(new RequestLogsFeature(50));

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

        private async Task SetupApplication(SetupInfo info = null)
        {
            var watch = new Stopwatch();

            // Depends will be either, ALL setup operations if info is null, or all the operations info depends on
            var depends = _operations;
            if (info != null)
                depends = depends.Where(x => info.Depends.Any() && info.Depends.Contains(x.Name));

            foreach (var depend in depends)
                await SetupApplication(depend).ConfigureAwait(false);

            if (info == null || info.Operation.Done)
                return;

            Logger.Info("**************************************************************");
            Logger.Info("   Running setup operation {0}.{1}".Fmt(info.Category, info.Name));
            Logger.Info("**************************************************************");

            watch.Start();
            if (!await info.Operation.Initialize().ConfigureAwait(false))
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
        public string Name { get; set; }

        public string[] Depends { get; set; }

        public string Category { get; set; }

        public ISetup Operation { get; set; }
    }
}