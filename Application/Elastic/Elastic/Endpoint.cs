
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
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);
        private static IContainer _container;
        private static NLog.ILogger Logger = LogManager.GetLogger("Elastic");
        private static Meter _exceptionsMeter = Metric.Meter("Exceptions Reported", Unit.Items);
        private static IEnumerable<SetupInfo> _operations;

        private static Int32 _buckets;
        private static Int32 _bucketsHandled;

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.ExceptionObject);
            Console.WriteLine(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }
        private static void ExceptionTrapper(object sender, FirstChanceExceptionEventArgs e)
        {
            _exceptionsMeter.Mark();
            //Logger.Debug(e.Exception, "Thrown exception: {0}");
        }

        private static void Main(string[] args)
        {

            ServicePointManager.UseNagleAlgorithm = false;

            var conf = NLog.Config.ConfigurationItemFactory.Default;
            conf.LayoutRenderers.RegisterDefinition("logsdir", typeof(LogsDir));
            conf.LayoutRenderers.RegisterDefinition("Domain", typeof(Library.Logging.Domain));
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration($"{AppDomain.CurrentDomain.BaseDirectory}/logging.config");

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            AppDomain.CurrentDomain.FirstChanceException += ExceptionTrapper;

            NServiceBus.Logging.LogManager.Use<NLogFactory>();

            var buckets = args.FirstOrDefault(x => x.StartsWith("--buckets"));
            var handled = args.FirstOrDefault(x => x.StartsWith("--handled"));

            _buckets = 1;
            _bucketsHandled = 1;
            try
            {
                _buckets = Int32.Parse(buckets.Substring(buckets.IndexOf('=') + 1), System.Globalization.NumberStyles.Integer);
                _bucketsHandled = Int32.Parse(handled.Substring(handled.IndexOf('=') + 1), System.Globalization.NumberStyles.Integer);
            }
            catch { }

            var client = ConfigureStore();
            var elastic = ConfigureElastic();
            ConfigureMetrics();

            _container = new Container(x =>
            {
                x.For<IManager>().Use<Manager>();
                x.For<IEventStoreConnection>().Use(client).Singleton();
                x.For<IFuture>().Use<Future>().Singleton();
                x.For<IPersistCheckpoints>().Use<Checkpoints.ElasticCheckpoints>();
                x.For<IManageCompetes>().Use<Checkpoints.ElasticCompetes>();
                x.For<IUnitOfWork>().Use<UnitOfWork>();
                x.For<IEventUnitOfWork>().Add(b => (IEventUnitOfWork)b.GetInstance<IUnitOfWork>());
                x.For<IElasticClient>().Use(elastic).Singleton();

                x.Scan(y =>
                {
                    AllAssemblies.Matching("Application.Elastic").ToList().ForEach(a => y.Assembly(a));

                    y.WithDefaultConventions();
                    y.AddAllTypesOf<ISetup>();
                });
            });
            // Do this before bus starts
            InitiateSetup();
            SetupApplication();

            var bus = InitBus();
            _container.Configure(x => x.For<IBus>().Use(bus).Singleton());


            Console.WriteLine("Press CTRL+C to exit...");
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };
            _quitEvent.WaitOne();
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
            if (String.IsNullOrEmpty(index))
                throw new ArgumentException("No DefaultIndex parameter in elastic connection string");

            index = index.Substring(13);

            var node = new Uri(url.Substring(4));
            var pool = new Elasticsearch.Net.SingleNodeConnectionPool(node);
            
            var regex = new Regex("([+\\-!\\(\\){}\\[\\]^\"~*?:\\\\\\/>< ]|[&\\|]{2}|AND|OR|NOT)", RegexOptions.Compiled);
            var settings = new Nest.ConnectionSettings(pool, (_) => new JsonNetSerializer(_))
                .DefaultIndex(index)
                .DefaultTypeNameInferrer(type =>
                        type.FullName
                        .Replace("Demo.Presentation.ServiceStack.", "")
                        .Replace("Demo.Application.Elastic.", "")
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
                            .TokenFilters(f => f.NGram("ngram", d => d.MinGram(2).MaxGram(15)))
                            .Analyzers(a => a
                                .Custom("default_index", t => t.Tokenizer("keyword").Filters(new[] { "standard", "lowercase", "asciifolding", "kstem", "ngram" }))
                                .Custom("suffix", t => t.Tokenizer("keyword").Filters(new[] { "standard", "lowercase", "asciifolding", "reverse" }))
                            )
                        )));
            
            return client;
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
                    return new IPEndPoint(IPAddress.Loopback, Int32.Parse(addr[1]));
                return new IPEndPoint(IPAddress.Parse(addr[0]), Int32.Parse(addr[1]));
            }).ToArray();
            
            
            var cred = new UserCredentials("admin", "changeit");
            var settings = EventStore.ClientAPI.ConnectionSettings.Create()
                .UseCustomLogger(new EventStoreLogger())
                .KeepReconnecting()
                .KeepRetrying()
                .SetHeartbeatInterval(TimeSpan.FromSeconds(30))
                .SetGossipTimeout(TimeSpan.FromSeconds(30))
                .SetHeartbeatTimeout(TimeSpan.FromMinutes(5))
                .SetTimeoutCheckPeriodTo(TimeSpan.FromMinutes(5))
                .SetDefaultUserCredentials(cred);
            
            var client = EventStoreConnection.Create(settings, endpoints.First(), "Elastic");

            client.ConnectAsync().Wait();

            return client;
        }
        private static IBus InitBus()
        {
            NServiceBus.Logging.LogManager.Use<NLogFactory>();

            var config = new BusConfiguration();

            Logger.Info("Initializing Service Bus");
            config.LicensePath(ConfigurationManager.AppSettings["license"]);

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            if (string.IsNullOrEmpty(endpoint))
                endpoint = "elastic";

            config.EndpointName(endpoint);

            config.EnableInstallers();
            config.UseTransport<RabbitMQTransport>()
                
                .UseDirectRoutingTopology()
                .ConnectionStringName("RabbitMq");

            config.Transactions().DisableDistributedTransactions();
            //config.DisableDurableMessages();
            config.UseSerialization<NewtonsoftSerializer>();

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));

            config.SetReadSize(100);
            config.MaxProcessingQueueSize(10000);
            config.SetBucketHeartbeats(5);
            config.SetBucketExpiration(20);
            config.ParallelHandlers(false);
            config.MaxRetries(20);
            config.SetParallelism(1);

            Logger.Info("Bucket configuration: {0} total {1} handled", _buckets, _bucketsHandled);
            config.SetBucketCount(_buckets);
            config.SetBucketsHandled(_bucketsHandled);

            config.EnableFeature<Aggregates.CompetingConsumer>();
            config.DisableFeature<Sagas>();
            config.DisableFeature<SecondLevelRetries>();
            config.DisableFeature<AutoSubscribe>();

            

            if (Logger.IsDebugEnabled)
                config.Pipeline.Register<LogIncomingRegistration>();

            return Bus.Create(config).Start();
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

        private static void SetupApplication(SetupInfo info = null)
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
            Logger.Info("   Running setup operation {0}.{1}", info.Category, info.Name);
            Logger.Info("**************************************************************");

            watch.Start();
            if (!info.Operation.Initialize())
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
        public String Name { get; set; }

        public String[] Depends { get; set; }

        public String Category { get; set; }

        public ISetup Operation { get; set; }
    }

}
