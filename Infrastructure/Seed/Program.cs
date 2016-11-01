using Demo.Library.Security;
using log4net;
using Nest;
using NServiceBus;
using Seed.Attributes;
using Seed.Operations;
using ServiceStack;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.Contracts;
using Demo.Library.Future;
using Demo.Library.Logging;
using Demo.Library.Setup;
using Metrics;
using NLog;
using NServiceBus.Features;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RiakClient;
using RiakClient.Models;
using Seed.Internal;

namespace Seed
{
    internal class SetupInfo
    {
        public string Name { get; set; }

        public string[] Depends { get; set; }

        public string Category { get; set; }

        public ISetup Operation { get; set; }
    }
    internal class ImportInfo
    {
        public string Name { get; set; }

        public string[] Depends { get; set; }

        public string Category { get; set; }

        public IImport Operation { get; set; }
    }

    internal class Program
    {
        static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static IEnumerable<SetupInfo> _setups;
        private static IEnumerable<ImportInfo> _imports;
        private static IContainer _container;
        private static readonly ILogger Logger = LogManager.GetLogger("Seed");

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.ExceptionObject);
            Console.WriteLine(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }
        private static void ExceptionTrapper(object sender, FirstChanceExceptionEventArgs e)
        {
            //Logger.Debug(e.Exception, "Thrown exception: {0}");
        }

        private static void Main(string[] args)
        {
            ServicePointManager.UseNagleAlgorithm = false;
            var conf = NLog.Config.ConfigurationItemFactory.Default;
            conf.LayoutRenderers.RegisterDefinition("logsdir", typeof(LogsDir));
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration($"{AppDomain.CurrentDomain.BaseDirectory}/logging.config");

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            AppDomain.CurrentDomain.FirstChanceException += ExceptionTrapper;
            
            _container = new Container(x =>
            {
                x.Scan(y =>
                {
                    y.TheCallingAssembly();
                    y.AddAllTypesOf<ISetup>();
                    y.AddAllTypesOf<IImport>();

                    y.AddAllTypesOf<ICommandMutator>();
                });
                x.For<IFuture>().Use<Future>();
                x.For<IManager>().Use<Manager>();
            });


            if ((args != null && args.Any() && args.Any(x => x == "/clear")))
            {
                ReseedDb();
                Environment.Exit(0);
            }

            var bus = InitBus().Result;
            _container.Configure(x => x.For<IMessageSession>().Use(bus).Singleton());
            LoadOperations();

            if ((args != null && args.Any() && args.Any(x => x == "/init")))
            {
                // Run all initial setups
                SetupCategory("*").Wait();
                Environment.Exit(0);
            }


            if (!(args != null && args.Any() && args.Any(x => x == "/y")))
            {
                Console.Clear();

                var maxMenuItems = 9;
                var good = false;
                var selector = 0;
                while (selector != maxMenuItems && selector != 3)
                {
                    //Console.Clear();
                    DoTitle();
                    DrawMenu(maxMenuItems);

                    good = int.TryParse(Console.ReadLine(), out selector);
                    if (good)
                    {
                        switch (selector)
                        {
                            case 1:
                                ReseedDb();
                                break;

                            case 2:
                                SetupCategory("*").Wait();
                                break;

                            case 3:
                                break;

                            default:
                                if (selector != maxMenuItems)
                                    Trace.WriteLine("Error - unknown selection");
                                break;
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Error - unknown input");
                    }
                }
                if (selector == maxMenuItems)
                    return;

            }
            else
                Thread.Sleep(TimeSpan.FromSeconds(10));

            ImportCategory("*").Wait();

            Console.WriteLine("Press CTRL+C to exit...");
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };


            QuitEvent.WaitOne();

            bus.Stop().Wait();
        }

        private static void DoTitle()
        {
            Console.WriteLine("Pulse import program for seeding example");
            Console.WriteLine("*****************************************************");
            Console.WriteLine();
            Console.WriteLine("Please make a selection");
        }

        private static void DrawMenu(int max)
        {
            Console.WriteLine(" 1. Clear Data");
            Console.WriteLine(" 2. Initialize");
            Console.WriteLine(" 3. Seed Data");

            // more here
            Console.WriteLine();
            Console.WriteLine(" {0}. Exit program", max);
            Console.WriteLine();
            Console.WriteLine("Make your choice: type 1, 2,... or {0} for exit", max);
        }

        private static async Task<IEndpointInstance> InitBus()
        {

            NServiceBus.Logging.LogManager.Use<NLogFactory>();

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            if (string.IsNullOrEmpty(endpoint))
                endpoint = "seed";

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
                //.UseRoutingTopology<ShardedRoutingTopology>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromSeconds(30));


            config.UseSerialization<NewtonsoftSerializer>();

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));


            config.EnableFeature<Aggregates.Feature>();
            config.Recoverability().ConfigureForAggregates();
            //config.EnableFeature<RoutedFeature>();
            config.DisableFeature<Sagas>();


            if (Logger.IsDebugEnabled)
                config.Pipeline.Register(
                    behavior: typeof(LogIncomingMessageBehavior),
                    description: "Logs incoming messages"
                    );

            return await NServiceBus.Endpoint.Start(config).ConfigureAwait(false);
        }
        
        private static void LoadOperations()
        {
            Logger.Debug("Loading all operations");

            _setups = _container.GetAllInstances<ISetup>().Select(o =>
            {
                var operation = o.GetType().GetCustomAttributes(typeof(OperationAttribute), true).FirstOrDefault() as OperationAttribute;
                var depends = o.GetType().GetCustomAttributes(typeof(DependsAttribute), true).FirstOrDefault() as DependsAttribute;
                var category = o.GetType().GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault() as CategoryAttribute;
                return new SetupInfo
                {
                    Name = operation.Name,
                    Depends = depends?.Depends,
                    Category = category.Name,
                    Operation = o,
                };
            });
            _imports = _container.GetAllInstances<IImport>().Select(o =>
            {
                var operation = o.GetType().GetCustomAttributes(typeof(OperationAttribute), true).FirstOrDefault() as OperationAttribute;
                var depends = o.GetType().GetCustomAttributes(typeof(DependsAttribute), true).FirstOrDefault() as DependsAttribute;
                var category = o.GetType().GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault() as CategoryAttribute;
                return new ImportInfo
                {
                    Name = operation.Name,
                    Depends = depends?.Depends,
                    Category = category.Name,
                    Operation = o,
                };
            });
        }

        private static async Task<bool> RunSetup(string operation, bool depends = true)
        {
            Logger.Debug($"Running operation '{operation}'");
            var op = _setups.SingleOrDefault(o => o.Name == operation);
            if (op == null)
            {
                Logger.Error($"Unknown operation '{operation}'");
                return false;
            }

            if (op.Operation.Done)
                return true;

            return await RunSetup(op.Operation, depends).ConfigureAwait(false);
        }
        private static async Task<bool> RunImport(string operation, bool depends = true)
        {
            Logger.Debug($"Running operation '{operation}'");
            var op = _imports.SingleOrDefault(o => o.Name == operation);
            if (op == null)
            {
                Logger.Error($"Unknown operation '{operation}'");
                return false;
            }

            if (op.Operation.Started)
                return true;

            return await RunImport(op.Operation, depends).ConfigureAwait(false);
        }

        private static async Task<bool> RunSetup(ISetup operation, bool Depends = true)
        {

            var depends = operation.GetType().GetCustomAttributes(typeof(DependsAttribute), true).FirstOrDefault() as DependsAttribute;

            if (Depends && depends != null)
            {
                foreach (var depend in depends.Depends)
                {
                    if (!await RunSetup(depend).ConfigureAwait(false))
                        Logger.Error($"Failed to run operation {depend}");
                }
            }

            var name = operation.GetType().GetCustomAttributes(typeof(OperationAttribute), true).FirstOrDefault() as OperationAttribute;

            var start = DateTime.UtcNow;
            Logger.Info("**************************************************************");
            Logger.Info($"   Running operation {name.Name}");
            Logger.Info("**************************************************************");


            if (!await operation.Initialize().ConfigureAwait(false))
            {
                Logger.Info("ERROR - Failed to seed entities!");
                return false;
            }

            Logger.Info("**************************************************************");
            Logger.Info($"   Finished operation {name.Name} in {DateTime.UtcNow - start}");
            Logger.Info("**************************************************************");

            return true;
        }
        private static async Task<bool> RunImport(IImport operation, bool depends = true)
        {

            var name = operation.GetType().GetCustomAttributes(typeof(OperationAttribute), true).FirstOrDefault() as OperationAttribute;

            var start = DateTime.UtcNow;
            Logger.Info("**************************************************************");
            Logger.Info($"   Running operation {name.Name}");
            Logger.Info("**************************************************************");


            if (!await operation.Import().ConfigureAwait(false))
            {
                Logger.Info("ERROR - Failed to seed entities!");
                return false;
            }

            Logger.Info("**************************************************************");
            Logger.Info($"   Finished operation {name.Name} in {DateTime.UtcNow - start}");
            Logger.Info("**************************************************************");

            return true;
        }

        private static bool ReseedDb()
        {
            ClearRabbit();
            ClearSql();
            ClearRaven();
            ClearElastic();
            ClearRiak();

            return true;
        }
        private static void ClearElastic()
        {

            Logger.Info("Deleting ElasticSearch index...");

            var connectionString = ConfigurationManager.ConnectionStrings["Elastic"];
            if (connectionString == null)
                return;

            var data = connectionString.ConnectionString.Split(';');

            var url = data.FirstOrDefault(x => x.StartsWith("Url", StringComparison.CurrentCultureIgnoreCase));
            if (url == null)
                throw new ArgumentException("No URL parameter in elastic connection string");
            var index = data.FirstOrDefault(x => x.StartsWith("DefaultIndex", StringComparison.CurrentCultureIgnoreCase));
            if (index.IsNullOrEmpty())
                throw new ArgumentException("No DefaultIndex parameter in elastic connection string");

            index = index.Substring(13);

            var node = new Uri(url.Substring(4));

            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);

            client.DeleteIndex($"{index}*");

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

            Logger.Info("Done");

        }
        private static void ClearRaven()
        {

            Logger.Info("Deleting all documents in RavenDB...");

            //ClearRavenDB("Raven");
            //ClearRavenDB("Raven.Domain");

            Logger.Info("Done");
        }
        private static void ClearRiak()
        {
            Logger.Info("Deleting all documents in Riak...");

            ClearRiakDb("Riak");
            ClearRiakDb("Riak.Domain");

            Logger.Info("Done");
        }
        private static void ClearSql()
        {

            Logger.Info("Deleting all sql tables");

            var connectionString = ConfigurationManager.ConnectionStrings["SQL"];
            if (connectionString == null)
                return;

            try
            {
                ClearDb(connectionString.ConnectionString);
            }
            catch (Exception e)
            {
                Logger.Warn(e, "Caught exception clearning SQL");
            }

            Logger.Info("Done");

        }
        private static void ClearRabbit()
        {


            var connectionString = ConfigurationManager.ConnectionStrings["RabbitMq"];
            if (connectionString == null)
                return;

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

            var uri = $"http://{host}:15672/api/queues";
            var queues = RabbitMqMgmt.GetAllQueues(uri, username, password, virtualhost);
            uri = $"http://{host}:15672/api/exchanges";
            var exchanges = RabbitMqMgmt.GetAllExchanges(uri, username, password, virtualhost);

            var factory = new ConnectionFactory { Uri = $"amqp://{username}:{password}@{host}:5672/{virtualhost}" };

            var conn = factory.CreateConnection();

            var channel = conn.CreateModel();

            Logger.Info("Deleting all RabbitMQ queues");
            foreach (var queue in queues)
            {
                try
                {
                    channel.QueuePurge(queue);
                    channel.QueueDeleteNoWait(queue, false, false);
                }
                catch (OperationInterruptedException) { }
            }
            Logger.Info("Deleting all RabbitMQ exchanges");
            foreach (var ex in exchanges)
            {
                if (string.IsNullOrEmpty(ex) || ex.StartsWith("amq")) continue;
                try
                {
                    channel.ExchangeDeleteNoWait(ex, false);
                }
                catch (OperationInterruptedException) { }
            }
        }

        private static void ClearDb(string connectionString)
        {
            var data = connectionString.Split(';');
            var initialCatalog = data.FirstOrDefault(x => x.StartsWith("initial catalog", StringComparison.CurrentCultureIgnoreCase));
            if (initialCatalog == null)
                throw new ArgumentException("No INITIAL CATALOG parameter in connection string");

            initialCatalog = initialCatalog.Substring(16);


            var safeConnection = string.Concat(data.Where(x => !x.StartsWith("initial catalog")).Join(";"), ";initial catalog=master");

            using (var con = new SqlConnection(safeConnection))
            {
                con.Open();

                var check = new SqlCommand("SELECT * from master.sys.sysdatabases WHERE name='{0}'".Fmt(initialCatalog), con);
                var results = check.ExecuteReader();
                if (results.HasRows)
                {
                    results.Close();
                    var drop = new SqlCommand("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{0}]".Fmt(initialCatalog), con);
                    drop.ExecuteNonQuery();
                }
                else
                    results.Close();


                var create = new SqlCommand("CREATE DATABASE [{0}]".Fmt(initialCatalog), con);
                create.ExecuteNonQuery();

                var user = ConfigurationManager.AppSettings["databaseUser"];
                if (!string.IsNullOrEmpty(user))
                {
                    var check2 = new SqlCommand("SELECT * from master.sys.server_principals WHERE name='{0}'".Fmt(user), con);
                    var results2 = check2.ExecuteReader();
                    if (!results2.HasRows)
                    {
                        results2.Close();
                        var createUser = new SqlCommand("CREATE LOGIN [{0}] FROM WINDOWS".Fmt(user), con);
                        createUser.ExecuteNonQuery();
                    }
                    else
                        results2.Close();

                    var createLogin = new SqlCommand(@"USE [{1}] CREATE USER [{0}] FOR LOGIN [{0}]; EXEC sp_addrolemember 'db_owner', '{0}'".Fmt(user, initialCatalog), con);
                    createLogin.ExecuteNonQuery();
                }

                con.Close();
            }
        }
        
        public static void ClearRiakDb(string connectionName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName];
            if (connectionString == null)
                return;

            var data = connectionString.ConnectionString.Split(';');

            var bucket = data.FirstOrDefault(x => x.StartsWith("bucket", StringComparison.CurrentCultureIgnoreCase));
            if (bucket == null)
                throw new ArgumentException("No BUCKET parameter in riak connection string");

            bucket = bucket.Substring(7);

            var cluster = RiakCluster.FromConfig("riakConfig");
            var client = cluster.CreateClient();

            var stream = client.StreamListKeys(bucket);
            var objectids = stream.Value.Select(x => new RiakObjectId(bucket, x)).ToList();
            client.Delete(objectids);

            var streamSystem = client.StreamListKeys($"{bucket}.system");
            var objectidsSystem = streamSystem.Value.Select(x => new RiakObjectId($"{bucket}.system", x)).ToList();
            client.Delete(objectidsSystem);

            var options = new RiakBucketProperties()
                .SetW(Quorum.WellKnown.Quorum).SetR(Quorum.WellKnown.Quorum)
                .SetDw(1).SetRw(Quorum.WellKnown.Quorum)
                .SetPr(1).SetPw(1)
                .SetLegacySearch(false);
            var result = client.SetBucketProperties(bucket, options);
            if (!result.IsSuccess)
                Logger.Warn("Failed to set primary bucket props.  Error: {0}", result.ErrorMessage);

            var systemOptions = new RiakBucketProperties()
                .SetW(Quorum.WellKnown.All).SetR(Quorum.WellKnown.All)
                .SetDw(1).SetRw(Quorum.WellKnown.All)
                .SetPr(Quorum.WellKnown.Quorum).SetPw(Quorum.WellKnown.Quorum)
                .SetLegacySearch(false);
            result = client.SetBucketProperties($"{bucket}.system", systemOptions);
            if (!result.IsSuccess)
                Logger.Warn("Failed to set system bucket props.  Error: {0}", result.ErrorMessage);

        }
        private static async Task<bool> ImportCategory(string category, bool depends = true)
        {
            var start = DateTime.UtcNow;

            Logger.Info("**************************************************************");
            Logger.Info($"   Started importing {category} - at {start}");
            Logger.Info("**************************************************************");

            foreach (var operation in _imports.Where(o => category == "*" || o.Category.ToLower() == category.ToLower()))
            {
                if (operation.Operation.Started)
                    continue;

                if (!await RunImport(operation.Operation, depends).ConfigureAwait(false))
                    Logger.Error($"Failed to run operation {operation.Name}");
            }

            Logger.Info("**************************************************************");
            Logger.Info($"   Finished importing {category} - took {DateTime.UtcNow - start}");
            Logger.Info("**************************************************************");

            return true;
        }
        private static async Task<bool> SetupCategory(string category, bool depends = true)
        {
            var start = DateTime.UtcNow;

            Logger.Info("**************************************************************");
            Logger.Info($"   Started importing {category} - at {start}");
            Logger.Info("**************************************************************");

            foreach (var operation in _setups.Where(o => category == "*" || o.Category.ToLower() == category.ToLower()))
            {
                if (operation.Operation.Done)
                    continue;

                if (!await RunSetup(operation.Operation, depends).ConfigureAwait(false))
                    Logger.Error($"Failed to run operation {operation.Name}");
            }

            Logger.Info("**************************************************************");
            Logger.Info($"   Finished importing {category} - took {DateTime.UtcNow - start}");
            Logger.Info("**************************************************************");

            return true;
        }
    }
}