using Demo.Library.Security;
using log4net;
using Nest;
using NServiceBus;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using Seed.Attributes;
using Seed.Operations;
using ServiceStack;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Seed
{
    internal class OperationInfo
    {
        public String Name { get; set; }

        public String[] Depends { get; set; }

        public String Category { get; set; }

        public IOperation Operation { get; set; }
    }


    internal class Program
    {
        private static IEnumerable<OperationInfo> _operations;
        private static IContainer _container;
        private static ILog _logger = LogManager.GetLogger("Seed");

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            log4net.Config.XmlConfigurator.Configure();


            _container = new Container(x =>
            {
                x.For<IOperation>().Add<Operations.AccountType>();
                x.For<IOperation>().Add<Operations.Currency>();
                x.For<IOperation>().Add<Operations.Country>();
                x.For<IOperation>().Add<Operations.Account>();
                x.For<IOperation>().Add<Operations.User>();

                x.For<IManager>().Use<Manager>();
            });
            var bus = InitBus();
            _container.Configure(x => x.For<IBus>().Use(bus).Singleton());

            LoadOperations();

            if (!(args != null && args.Any() && args.Any(x => x == "/y")))
            {
                Console.Clear();
                Console.WriteLine("***********************************");
                Console.WriteLine("*** Press enter to start import ***");
                Console.WriteLine("***********************************");
                Console.ReadLine();
            }

            // By pass user selection, just seed everything
            ReseedDB().Wait();
            ImportCategory("*").Wait();

            //var maxMenuItems = 9;
            //var good = false;
            //var selector = 0;

            //while (selector != maxMenuItems)
            //{
            //    //Console.Clear();
            //    DoTitle();
            //    DrawMenu(maxMenuItems);

            //    good = Int32.TryParse(Console.ReadLine(), out selector);
            //    if (good)
            //    {
            //        switch (selector)
            //        {
            //            case 1:
            //                ReseedDB().Wait();
            //                break;

            //            case 2:
            //                Console.WriteLine("Please enter the category name to import: ");
            //                var category = Console.ReadLine();
            //                ImportCategory(category).Wait();
            //                break;

            //            case 3:
            //            case 4:
            //                Console.WriteLine("Please enter the operation name to import: ");
            //                var op = Console.ReadLine();
            //                RunOperation(op, (selector == 4)).Wait();
            //                break;

            //            case 8:
            //                ReseedDB().Wait();
            //                ImportCategory("*").Wait();
            //                break;

            //            default:
            //                if (selector != maxMenuItems)
            //                    Console.WriteLine("Error - unknown selection");
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("Error - unknown input");
            //    }
            //}
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void DoTitle()
        {
            Console.WriteLine("Demo import program for seeding example");
            Console.WriteLine("*****************************************************");
            Console.WriteLine();
            Console.WriteLine("Please make a selection");
        }

        private static void DrawMenu(Int32 Max)
        {
            Console.WriteLine(" 1. Reseed Database");
            Console.WriteLine(" 2. Import category...");
            Console.WriteLine(" 3. Import specific...");
            Console.WriteLine(" 4. Import specific with depends...");
            Console.WriteLine(" 8. Import everything");

            // more here
            Console.WriteLine();
            Console.WriteLine(" 9. Exit program");
            Console.WriteLine();
            Console.WriteLine("Make your choice: type 1, 2,... or {0} for exit", Max);
        }

        private static IBus InitBus()
        {
            _logger.Debug("Initializing Service Bus");
            NServiceBus.Logging.LogManager.Use<NServiceBus.Log4Net.Log4NetFactory>();

            var config = new BusConfiguration();
            //var conventions = config.Conventions();
            //conventions
            //    .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Events"))
            //    .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Commands"))
            //    .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && (t.Namespace.EndsWith("Messages") || t.Namespace.EndsWith("Queries")));

            config.LicensePath(@"C:\License.xml");

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            if (string.IsNullOrEmpty(endpoint))
                endpoint = "seed";

            config.EndpointName(endpoint);
            //config.AssembliesToScan(AllAssemblies.Matching("Presentation").And("Application").And("Domain").And("Library"));

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));
            config.UseSerialization<NServiceBus.JsonSerializer>();

            config.EnableInstallers();

            return Bus.Create(config).Start();
        }

        private static void LoadOperations()
        {
            _logger.Debug("Loading all operations");

            _operations = _container.GetAllInstances<IOperation>().Select(o =>
            {
                var operation = o.GetType().GetCustomAttributes(typeof(OperationAttribute), true).FirstOrDefault() as OperationAttribute;
                var depends = o.GetType().GetCustomAttributes(typeof(DependsAttribute), true).FirstOrDefault() as DependsAttribute;
                var category = o.GetType().GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault() as CategoryAttribute;
                return new OperationInfo
                {
                    Name = operation.Name,
                    Depends = depends == null ? null : depends.Depends,
                    Category = category.Name,
                    Operation = o
                };
            });
        }

        private static async Task<Boolean> RunOperation(String operation, Boolean Depends = true)
        {
            _logger.Debug("Running operation '{0}'".Fmt(operation));
            var op = _operations.SingleOrDefault(o => o.Name == operation);
            if (op == null)
            {
                _logger.Error("Unknown operation '{0}'".Fmt(operation));
                return false;
            }

            if (op.Operation.Done)
                return true;

            return await RunOperation(op.Operation, Depends);
        }

        private static async Task<Boolean> RunOperation(IOperation operation, Boolean Depends = true)
        {
            var watch = new Stopwatch();
            var start = DateTime.UtcNow;

            var depends = operation.GetType().GetCustomAttributes(typeof(DependsAttribute), true).FirstOrDefault() as DependsAttribute;

            if (Depends && depends != null)
            {
                foreach (var depend in depends.Depends)
                {
                    if (!await RunOperation(depend))
                        _logger.Error("Failed to run operation {0}".Fmt(depend));
                }
            }

            var name = operation.GetType().GetCustomAttributes(typeof(OperationAttribute), true).FirstOrDefault() as OperationAttribute;

            _logger.Info("**************************************************************");
            _logger.Info("   Running operation {0}".Fmt(name.Name));
            _logger.Info("**************************************************************");

            _logger.Info("Seeding entities...");

            watch.Start();
            if (!await operation.Seed())
            {
                _logger.Info("ERROR - Failed to seed entities!");
                return false;
            }
            watch.Stop();
            _logger.Info("Seeded entities in {0}".Fmt(watch.Elapsed));
            watch.Reset();

            _logger.Info("**************************************************************");
            _logger.Info("   Finished operation {0} in {1}".Fmt(name.Name, DateTime.UtcNow - start));
            _logger.Info("**************************************************************");

            return true;
        }

        private static async Task<Boolean> ReseedDB()
        {
            _logger.Info("Purging all MSMQ queues...");
            var msmques = MessageQueue.GetPrivateQueuesByMachine(".");
            foreach (var queue in msmques)
                queue.Purge();
            _logger.Info("Done");

            _logger.Info("Deleting all Databases...");
            {
                _logger.Info("Deleting all documents in RavenDB...");

                var connectionString = ConfigurationManager.ConnectionStrings["Raven"];
                if (connectionString == null)
                    throw new ArgumentException("No Raven connection string found");

                var data = connectionString.ConnectionString.Split(';');

                var url = data.FirstOrDefault(x => x.StartsWith("Url", StringComparison.CurrentCultureIgnoreCase));
                if (url == null)
                    throw new ArgumentException("No URL parameter in elastic connection string");
                var db = data.FirstOrDefault(x => x.StartsWith("Database", StringComparison.CurrentCultureIgnoreCase));
                if (db.IsNullOrEmpty())
                    throw new ArgumentException("No Database parameter in elastic connection string");

                db = db.Substring(9);



                using (var store = new DocumentStore { ConnectionStringName = "Raven" }.Initialize())
                {
                    store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(db);

                    using (var session = store.OpenSession())
                    {
                        session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex("Raven/DocumentsByEntityName",
                            new IndexQuery { Query = "Tag:*" }
                        );
                    }
                }
                _logger.Info("Done");
            }
            {
                _logger.Info("Deleting ElasticSearch index...");

                var connectionString = ConfigurationManager.ConnectionStrings["Elastic"];
                if (connectionString == null)
                    throw new ArgumentException("No elastic connection string found");

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

                client.DeleteIndex(index);

                client.CreateIndex(index, i => i
                        .Analysis(analysis => analysis
                            .TokenFilters(f => f
                                .Add("ngram", new Nest.NgramTokenFilter { MinGram = 2, MaxGram = 15 })
                                )
                            .Analyzers(a => a
                                .Add(
                                    "default",
                                    new Nest.CustomAnalyzer
                                    {
                                        Tokenizer = "standard",
                                        Filter = new[] { "standard", "lowercase", "asciifolding", "kstem", "ngram" }
                                    }
                                )
                                .Add(
                                    "suffix",
                                    new Nest.CustomAnalyzer
                                    {
                                        Tokenizer = "keyword",
                                        Filter = new[] { "standard", "lowercase", "asciifolding", "reverse" }
                                    }
                                )
                            )
                        ));

                _logger.Info("Done");
            }

            await ImportCategory("configuration");

            return true;
        }

        private static async Task<Boolean> ImportCategory(String category)
        {
            var start = DateTime.UtcNow;

            _logger.Info("**************************************************************");
            _logger.Info("   Started importing {0} - at {1}".Fmt(category, start));
            _logger.Info("**************************************************************");

            foreach (var operation in _operations.Where(o => category == "*" || o.Category.ToLower() == category.ToLower()))
            {
                if (operation.Operation.Done)
                    continue;

                if (!await RunOperation(operation.Operation))
                    Trace.WriteLine("Failed to run operation {0}".Fmt(operation.Name));
            }

            _logger.Info("**************************************************************");
            _logger.Info("   Finished importing {0} - took {1}".Fmt(category, DateTime.UtcNow - start));
            _logger.Info("**************************************************************");

            return true;
        }
    }
}