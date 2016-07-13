
namespace Demo.Application.Riak
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
    using Infrastructure;
    using RiakClient;
    using Infrastructure.Riak;
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
        private static NLog.ILogger Logger = LogManager.GetLogger("Riak");
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
            var riak = ConfigureRiak();
            ConfigureMetrics();

            _container = new Container(x =>
            {
                x.For<IManager>().Use<Manager>();
                x.For<IEventStoreConnection>().Use(client).Singleton();
                x.For<IFuture>().Use<Future>().Singleton();
                x.For<IPersistCheckpoints>().Use<Checkpoints.RiakCheckpoints>();
                x.For<IManageCompetes>().Use<Checkpoints.RiakCompetes>();
                x.For<IUnitOfWork>().Use<UnitOfWork>();
                x.For<IEventUnitOfWork>().Add(b => (IEventUnitOfWork)b.GetInstance<IUnitOfWork>());
                x.For<IRiakClient>().Use(riak).Singleton();

                x.Scan(y =>
                {
                    AllAssemblies.Matching("Application.Riak").ToList().ForEach(a => y.Assembly(a));

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
        
        public static IRiakClient ConfigureRiak()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Riak"];
            if (connectionString == null)
                throw new ArgumentException("No Riak connection string");

            var data = connectionString.ConnectionString.Split(';');

            var bucket = data.FirstOrDefault(x => x.StartsWith("bucket", StringComparison.CurrentCultureIgnoreCase));
            if (bucket == null)
                throw new ArgumentException("No BUCKET parameter in riak connection string");

            bucket = bucket.Substring(7);

            var cluster = RiakCluster.FromConfig("riakConfig");
            var client = cluster.CreateClient();

            Settings.Bucket = bucket;
            var regex = new Regex("([+\\-!\\(\\){}\\[\\]^\"~*?:\\\\\\/>< ]|[&\\|]{2}|AND|OR|NOT)", RegexOptions.Compiled);
            Settings.KeyGenerator = (type, key) =>
            {
                var first = regex.Replace(type.FullName.Replace("Demo.Presentation.ServiceStack.", "").Replace("Demo.Application.Riak.", ""), "");
                return $"{first}:{key}";
            };

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
            var settings = ConnectionSettings.Create()
                .UseCustomLogger(new EventStoreLogger())
                .KeepReconnecting()
                .KeepRetrying()
                .SetHeartbeatInterval(TimeSpan.FromSeconds(30))
                .SetGossipTimeout(TimeSpan.FromSeconds(30))
                .SetHeartbeatTimeout(TimeSpan.FromMinutes(5))
                .SetTimeoutCheckPeriodTo(TimeSpan.FromMinutes(5))
                .SetDefaultUserCredentials(cred);

            var client = EventStoreConnection.Create(settings, endpoints.First(), "Riak");

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
                endpoint = "riak";

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
            config.ParallelHandlers(true);
            config.SetBucketHeartbeats(5);
            config.SetBucketExpiration(20);
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
