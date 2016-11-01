
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Demo.Application.Elastic
{
    using EventStore.ClientAPI;
    using EventStore.ClientAPI.SystemData;
    using global::Aggregates.Contracts;
    using log4net;
    using NServiceBus;
    using NServiceBus.Newtonsoft.Json;
    using StructureMap;
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using Metrics;
    using System.Threading;
    using Library.Security;
    using Library.Future;
    using NServiceBus.Features;
    using Aggregates;
    using Library.GES;
    using Library.Logging;
    using Nest;
    using Infrastructure;
    using Library.Setup;
    using Library.Setup.Attributes;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.ExceptionServices;
    using System.IO;
    using System.Text.RegularExpressions;
    using NLog;
    using NServiceBus.UnitOfWork;



    internal class Program
    {
        static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static IContainer _container;
        private static readonly NLog.ILogger Logger = LogManager.GetLogger("Elastic");
        private static readonly Meter ExceptionsMeter = Metric.Meter("Exceptions Reported", Unit.Items);
        private static IEnumerable<SetupInfo> _operations;


        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.ExceptionObject);
            Console.WriteLine(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }
        private static void ExceptionTrapper(object sender, FirstChanceExceptionEventArgs e)
        {
            ExceptionsMeter.Mark();
            //Logger.Debug(e.Exception, "Thrown exception: {0}");
        }

        private static void Main(string[] args)
        {

            ServicePointManager.UseNagleAlgorithm = false;

            var conf = NLog.Config.ConfigurationItemFactory.Default;
            conf.LayoutRenderers.RegisterDefinition("logsdir", typeof(LogsDir));
            conf.LayoutRenderers.RegisterDefinition("Instance", typeof(Library.Logging.Instance));
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration($"{AppDomain.CurrentDomain.BaseDirectory}/logging.config");

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            AppDomain.CurrentDomain.FirstChanceException += ExceptionTrapper;

            NServiceBus.Logging.LogManager.Use<NLogFactory>();

            //var client = ConfigureStore();
            var elastic = ConfigureElastic();
            var rabbit = ConfigureRabbit();

            _container = new Container(x =>
            {
                x.For<IManager>().Use<Manager>();
                //x.For<IEventStoreConnection>().Use(client).Singleton();
                x.For<IFuture>().Use<Future>().Singleton();
                x.For<IPersistCheckpoints>().Use<Checkpoints.ElasticCheckpoints>();
                x.For<IManageCompetes>().Use<Checkpoints.ElasticCompetes>();
                x.For<IUnitOfWork>().Use<UnitOfWork>();
                x.For<IEventUnitOfWork>().Add(b => (IEventUnitOfWork)b.GetInstance<IUnitOfWork>());
                x.For<IElasticClient>().Use(elastic).Singleton();
                x.For<IConnection>().Use(rabbit).Singleton();

                x.Scan(y =>
                {
                    y.TheCallingAssembly();
                    y.AssembliesFromApplicationBaseDirectory((assembly) => assembly.FullName.StartsWith("Application.Elastic"));

                    y.WithDefaultConventions();
                    y.AddAllTypesOf<ISetup>();
                    y.AddAllTypesOf<IEventMutator>();
                });
            });
            // Do this before bus starts
            InitiateSetup();
            SetupApplication().Wait();

            var bus = InitBus().Result;
            _container.Configure(x => x.For<IMessageSession>().Use(bus).Singleton());

            Console.WriteLine("Press CTRL+C to exit...");
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };
            QuitEvent.WaitOne();

            bus.Stop().Wait();
        }
        
        public static IElasticClient ConfigureElastic()
        {
            Logger.Info("Setting up Elastic");
            var connectionString = ConfigurationManager.ConnectionStrings["Elastic"];
            if (connectionString == null)
                throw new ArgumentException("No elastic connection string found");

            var data = connectionString.ConnectionString.Split(';');

            var url = data.FirstOrDefault(x => x.StartsWith("Url", StringComparison.CurrentCultureIgnoreCase));
            if (url == null)
                throw new ArgumentException("No URL parameter in elastic connection string");
            var index = data.FirstOrDefault(x => x.StartsWith("DefaultIndex", StringComparison.CurrentCultureIgnoreCase));
            if (string.IsNullOrEmpty(index))
                throw new ArgumentException("No DefaultIndex parameter in elastic connection string");

            index = index.Substring(13);

            var node = new Uri(url.Substring(4));
            var pool = new Elasticsearch.Net.SingleNodeConnectionPool(node);

            var regex = new Regex("([+\\-!\\(\\){}\\[\\]^\"~*?:\\\\\\/>< ]|[&\\|]{2}|AND|OR|NOT)", RegexOptions.Compiled);
            var settings = new Nest.ConnectionSettings(pool, (_) => new JsonNetSerializer(_))
                .DefaultIndex(index)
                .DefaultTypeNameInferrer(type =>
                        type.FullName
                        .Replace("Pulse.Presentation.ServiceStack.", "")
                        .Replace("Pulse.Application.Elastic.", "")
                        .Replace('.', '_')
                        )
                .DefaultFieldNameInferrer(field => regex.Replace(field, ""));
#if DEBUG
            settings = settings.DisableDirectStreaming();
#endif

            var client = new ElasticClient(settings);


            if (!client.IndexExists(index).Exists)
                client.CreateIndex(index, i => i
                    .Settings(s => s
                        .NumberOfShards(8)
                        .NumberOfReplicas(0)
                        .Analysis(analysis => analysis
                        .TokenFilters(f => f.NGram("ngram", d => d.MinGram(1).MaxGram(15)))
                        .Analyzers(a => a
                            .Custom("default_index", t => t.Tokenizer("keyword").Filters(new[] { "standard", "lowercase", "asciifolding", "kstem", "ngram" }))
                            .Custom("suffix", t => t.Tokenizer("keyword").Filters(new[] { "standard", "lowercase", "asciifolding", "reverse" }))
                        )
                    )));

            return client;
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

            var factory = new ConnectionFactory { Uri = $"amqp://{username}:{password}@{host}:5672/{virtualhost}" };

            return factory.CreateConnection();
        }

        public static IEventStoreConnection ConfigureStore()
        {
            Logger.Info("Setting up Eventstore");
            var connectionString = ConfigurationManager.ConnectionStrings["EventStore"];
            var data = connectionString.ConnectionString.Split(';');

            var hosts = data.Where(x => x.StartsWith("Host", StringComparison.CurrentCultureIgnoreCase));

            if (!hosts.Any())
                throw new ArgumentException("No Host parameter in eventstore connection string");

            var endpoints = hosts.Select(x =>
            {
                var addr = x.Substring(5).Split(':');
                if (addr[0] == "localhost")
                    return new IPEndPoint(IPAddress.Loopback, int.Parse(addr[1]));
                return new IPEndPoint(IPAddress.Parse(addr[0]), int.Parse(addr[1]));
            }).ToArray();

            var cred = new UserCredentials("admin", "changeit");
            var settings = EventStore.ClientAPI.ConnectionSettings.Create()
                .UseCustomLogger(new EventStoreLogger())
                .KeepReconnecting()
                .KeepRetrying()
                .SetClusterGossipPort(endpoints.First().Port - 1)
                .SetHeartbeatInterval(TimeSpan.FromSeconds(30))
                .SetGossipTimeout(TimeSpan.FromMinutes(5))
                .SetHeartbeatTimeout(TimeSpan.FromMinutes(5))
                .SetTimeoutCheckPeriodTo(TimeSpan.FromMinutes(1))
                .SetDefaultUserCredentials(cred);

            IEventStoreConnection client;
            if (hosts.Count() != 1)
            {

                settings = settings
                    .SetGossipSeedEndPoints(endpoints);

                var clusterSettings = EventStore.ClientAPI.ClusterSettings.Create()
                    .DiscoverClusterViaGossipSeeds()
                    .SetGossipSeedEndPoints(endpoints.Select(x => new IPEndPoint(x.Address, x.Port - 1)).ToArray())
                    .SetGossipTimeout(TimeSpan.FromMinutes(5))
                    .Build();

                client = EventStoreConnection.Create(settings, clusterSettings, "Elastic");
            }
            else
                client = EventStoreConnection.Create(settings, endpoints.First(), "Elastic");

            client.ConnectAsync().Wait();

            return client;
        }
        private static async Task<IEndpointInstance> InitBus()
        {
            NServiceBus.Logging.LogManager.Use<NLogFactory>();

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            if (string.IsNullOrEmpty(endpoint))
                endpoint = "elastic";

            var config = new EndpointConfiguration(endpoint);
            config.MakeInstanceUniquelyAddressable(Defaults.Instance.ToString());

            Logger.Info("Initializing Service Bus");
            config.LicensePath(ConfigurationManager.AppSettings["license"]);

            config.EnableInstallers();
            config.LimitMessageProcessingConcurrencyTo(1);
            config.UseTransport<RabbitMQTransport>()
                //.CallbackReceiverMaxConcurrency(4)
                //.UseDirectRoutingTopology()
                .ConnectionStringName("RabbitMq")
                .PrefetchMultiplier(50)
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromSeconds(30));


            config.UseSerialization<NewtonsoftSerializer>();

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));

            config.SetReadSize(100);
            config.SlowAlertThreshold(1000);

            if (Logger.IsDebugEnabled)
            {
                config.EnableSlowAlerts(true);
                //config.EnableCriticalTimePerformanceCounter();
                config.Pipeline.Register(
                    behavior: typeof(LogIncomingMessageBehavior),
                    description: "Logs incoming messages"
                    );
            }

            config.EnableFeature<Aggregates.ConsumerFeature>();
            config.Recoverability().ConfigureForAggregates();
            //config.EnableFeature<RoutedFeature>();
            config.DisableFeature<Sagas>();


            return await Endpoint.Start(config).ConfigureAwait(false);
        }
        private static void InitiateSetup()
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

        private static async Task SetupApplication(SetupInfo info = null)
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
            Logger.Info("   Running setup operation {0}.{1}", info.Category, info.Name);
            Logger.Info("**************************************************************");

            watch.Start();
            if (!await info.Operation.Initialize().ConfigureAwait(false))
            {
                Logger.Info("ERROR - Failed to complete setup operation!");
                Environment.Exit(1);
            }
            watch.Stop();
            Logger.Info("**************************************************************");
            Logger.Info("   Finished operation {0}.{1} in {2}!", info.Category, info.Name, watch.Elapsed);
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
