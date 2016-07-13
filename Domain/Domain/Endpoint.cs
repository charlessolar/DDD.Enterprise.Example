namespace Demo.Domain
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
    using Library.Metrics;
    using System.Threading;
    using Library.Security;
    using Library.Future;
    using NServiceBus.Features;
    using Aggregates;
    using Library.GES;
    using Library.Logging;
    using EventStore.ClientAPI.Projections;
    using System.Net.Sockets;
    using Infrastructure.AggregatesNet;
    using Infrastructure.GES;
    using System.Threading.Tasks;
    using RiakClient;
    using RiakClient.Models;
    using Infrastructure.Riak;
    using System.Runtime.ExceptionServices;
    using System.IO;
    using System.Text.RegularExpressions;
    using NLog;
    using EventStore.ClientAPI.Embedded;
    using EventStore.Core.Services.Transport.Http.Controllers;
    using EventStore.Core;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Library.Setup.Attributes;
    using Library.Setup;
    internal class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);
        private static IContainer _container;
        private static NLog.ILogger Logger = LogManager.GetLogger("Domain");
        private static Meter _exceptionsMeter = Metric.Meter("Exceptions Reported", Unit.Items);
        private static IEnumerable<SetupInfo> _operations;

        private static Boolean _embedded;

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
            //EventStore.Common.Log.LogManager.SetLogFactory((name) => new EmbeddedLogger(name));

            var embedded = args.FirstOrDefault(x => x.StartsWith("--embedded"));
            try
            {
                _embedded = Boolean.Parse(embedded.Substring(embedded.IndexOf('=') + 1));
            }
            catch { }

            var client = ConfigureStore();
            
            var riak = ConfigureRiak();
            ConfigureMetrics();

            _container = new Container(x =>
            {
                x.For<IManager>().Use<Manager>();
                x.For<IEventStoreConnection>().Use(client).Singleton();
                x.For<IEventMutator>().Use<EventMutator>();
                x.For<IFuture>().Use<Future>().Singleton();
                x.For<Infrastructure.IUnitOfWork>().Use<UnitOfWork>();
                x.For<ICommandUnitOfWork>().Add(b => (ICommandUnitOfWork)b.GetInstance<Infrastructure.IUnitOfWork>());
                x.For<IEventUnitOfWork>().Add(b => (IEventUnitOfWork)b.GetInstance<Infrastructure.IUnitOfWork>());
                x.For<IRiakClient>().Use(riak).Singleton();

                x.Scan(y =>
                {
                    AllAssemblies.Matching("Domain").ToList().ForEach(a => y.Assembly(a));

                    y.WithDefaultConventions();
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
        private static IBus InitBus()
        {
            NServiceBus.Logging.LogManager.Use<NLogFactory>();

            var config = new BusConfiguration();

            Logger.Info("Initializing Service Bus");
            config.LicensePath(ConfigurationManager.AppSettings["license"]);

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            if (string.IsNullOrEmpty(endpoint))
                endpoint = "domain";

            config.EndpointName(endpoint);

            config.EnableInstallers();
            config.UseTransport<RabbitMQTransport>()
                //.CallbackReceiverMaxConcurrency(4)
                .UseDirectRoutingTopology()
                .ConnectionStringName("RabbitMq");

            config.Transactions().DisableDistributedTransactions();
            //config.DisableDurableMessages();
            config.UseSerialization<NewtonsoftSerializer>();

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));

            config.EnableFeature<Aggregates.Domain>();
            config.EnableFeature<Aggregates.GetEventStore.Feature>();
            config.DisableFeature<Sagas>();
            config.DisableFeature<SecondLevelRetries>();
            config.DisableFeature<AutoSubscribe>();

            config.SetReadSize(100);
            config.MaxProcessingQueueSize(10000);
            config.ParallelHandlers(true);
            config.ShouldCacheEntities(true);
            config.MaxRetries(20);
            config.SetParallelism(1);

            


            if (Logger.IsDebugEnabled)
                config.Pipeline.Register<LogIncomingRegistration>();

            return Bus.Create(config).Start();
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
                var first = regex.Replace(type.FullName.Replace("Demo.Domain.", ""), "");
                return $"{first}:{key}";
            };

            return client;
        }
        
        public static IEventStoreConnection ConfigureStore()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["EventStore"];
            var data = connectionString.ConnectionString.Split(';');

            var host = data.FirstOrDefault(x => x.StartsWith("Host=", StringComparison.CurrentCultureIgnoreCase));
            if (String.IsNullOrEmpty(host))
                throw new ArgumentException("No Host parameter in eventstore connection string");
            var port = data.FirstOrDefault(x => x.StartsWith("Port=", StringComparison.CurrentCultureIgnoreCase));
            var disk = data.FirstOrDefault(x => x.StartsWith("Disk=", StringComparison.CurrentCultureIgnoreCase));
            var embedded = data.FirstOrDefault(x => x.StartsWith("Embedded=", StringComparison.CurrentCultureIgnoreCase));
            var path = data.FirstOrDefault(x => x.StartsWith("Path=", StringComparison.CurrentCultureIgnoreCase));

            var gossips = data.Where(x => x.StartsWith("Gossip", StringComparison.CurrentCultureIgnoreCase));

            var gossipEndpoints = gossips.Select(x =>
            {
                var addr = x.Substring(7).Split(':');
                return new IPEndPoint(IPAddress.Parse(addr[0]), Int32.Parse(addr[1]) - 2);
            }).ToArray();
            var gossipSeeds = gossips.Select(x =>
            {
                var addr = x.Substring(7).Split(':');
                return new IPEndPoint(IPAddress.Parse(addr[0]), Int32.Parse(addr[1]) - 1);
            }).ToArray();

            host = host.Substring(5);
            port = port?.Substring(5);
            disk = disk?.Substring(5) ?? "false";
            embedded = embedded?.Substring(9) ?? "false";
            path = path?.Substring(5);

            Int32 tcpExtPort = 2112;
            if (!String.IsNullOrEmpty(port))
                if (!Int32.TryParse(port, out tcpExtPort))
                    throw new ArgumentException("Port parameter invalid");

#if LOCAL
            var ip = IPAddress.Loopback;
#else
                // The dns lookup trick won't work inside containers
                var ip = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                        .First(x => x.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                        .GetIPProperties()
                        .UnicastAddresses
                        .First(x => x.Address.AddressFamily == AddressFamily.InterNetwork)
                        .Address.MapToIPv4();
#endif

            Logger.Info("Detected ip address: {0}", ip);
            if (gossipEndpoints != null && gossipEndpoints.Any())
                Logger.Info("Gossip seeds: {0}", gossipEndpoints.Select(x => x.ToString()).Aggregate((cur, next) => $"{cur}, {next}"));
            if ((embedded == "true" || _embedded) && (!gossipEndpoints.Any() || gossipEndpoints.Any(x => x.Address.ToString() == ip.ToString())))
            {

                Logger.Info("Starting event store on {0} port {1}", ip, tcpExtPort);

                var builder = EmbeddedVNodeBuilder
#if LOCAL
                                    .AsSingleNode()
#else
                                    .AsClusterMember(gossipEndpoints.Count())
                                    .WithGossipSeeds(gossipEndpoints)
#endif
                                .WithExternalHeartbeatInterval(TimeSpan.FromSeconds(30))
                                .WithInternalHeartbeatInterval(TimeSpan.FromSeconds(30))
                                .WithExternalHeartbeatTimeout(TimeSpan.FromMinutes(5))
                                .WithInternalHeartbeatTimeout(TimeSpan.FromMinutes(5))
                                .WithInternalTcpOn(new IPEndPoint(ip, tcpExtPort + 1))
                                .WithExternalTcpOn(new IPEndPoint(ip, tcpExtPort))
                                .WithInternalHttpOn(new IPEndPoint(ip, tcpExtPort - 2))
                                .WithExternalHttpOn(new IPEndPoint(ip, tcpExtPort - 1))
                                .AddExternalHttpPrefix($"http://*:{tcpExtPort - 1 }/")
                                .AddInternalHttpPrefix($"http://*:{tcpExtPort - 2 }/");


                if (disk == "true")
                {
                    System.IO.Directory.CreateDirectory($"{path}/{Aggregates.Defaults.Domain}");
                    builder = builder.RunOnDisk($"{path}/{Aggregates.Defaults.Domain}");
                }
                else
                {
                    builder = builder.RunInMemory();
                }

                var embeddedBuilder = builder.Build();
                if (embeddedBuilder.InternalHttpService != null)
                    embeddedBuilder.InternalHttpService.SetupController(new ClusterWebUiController(embeddedBuilder.MainQueue, new[] { NodeSubsystems.Projections }));
                embeddedBuilder.ExternalHttpService.SetupController(new ClusterWebUiController(embeddedBuilder.MainQueue, new[] { NodeSubsystems.Projections }));

                var tcs = new TaskCompletionSource<Boolean>();

                embeddedBuilder.NodeStatusChanged += (_, e) =>
                {
                    Logger.Info("EventStore status changed: {0}", e.NewVNodeState);
                    if (!tcs.Task.IsCompleted && (e.NewVNodeState == EventStore.Core.Data.VNodeState.Master ||
                                                    e.NewVNodeState == EventStore.Core.Data.VNodeState.Slave ||
                                                    e.NewVNodeState == EventStore.Core.Data.VNodeState.Manager))
                    {
                        tcs.SetResult(true);
                    }
                };
                embeddedBuilder.Start();

                tcs.Task.Wait();

            }

            IPAddress hostAddress;
            if (host.Equals("localhost", StringComparison.CurrentCultureIgnoreCase))
                hostAddress = IPAddress.Loopback;
            else
                if (!IPAddress.TryParse(host, out hostAddress))
                throw new ArgumentException("Host parameter invalid");

            Logger.Info("Starting eventstore connection to {0}", hostAddress);
            var endpoint = new IPEndPoint(hostAddress, tcpExtPort);
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
            
            var client = EventStoreConnection.Create(settings, endpoint, "Domain");

            client.ConnectAsync().Wait();

            return client;
        }
        

        public void SpecifyOrder(Order order)
        {
            //order.Specify(First<ValidationMessageHandler>.Then<SecurityMessageHandler>());
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