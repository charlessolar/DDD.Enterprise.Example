using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Aggregates;
using Demo.Application.Servicestack.RavenDB;
using Demo.Library.IoC;
using Demo.Library.Queries.Processor;
using Demo.Library.Security;
using Demo.Library.SSE;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Nest;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Logging.Log4Net;
using ServiceStack.Text;
using ServiceStack.Validation;
using StructureMap;
using Q = Demo.Library.Queries;
using R = Demo.Library.Responses;

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

        public IDocumentStore ConfigureStore()
        {
            var store = new DocumentStore { ConnectionStringName = "Raven" }.Initialize();
            //var store = new EmbeddableDocumentStore { ConnectionStringName = "Demo" }.Initialize();
            //store.Listeners.RegisterListener(new Versioning());

            store.Conventions.FindTypeTagName =
                type => type.FullName.Replace("Demo.Application.ServiceStack.", "").Replace('.', '_');

            return store;
        }

        public IEventStoreConnection ConfigureEventStore()
        {
            var tcpPort = ConfigurationManager.AppSettings["eventstoreTcp"];

            Int32 tcpExtPort = 1113;

            Int32.TryParse(tcpPort, out tcpExtPort);

            var endpoint = new IPEndPoint(IPAddress.Loopback, tcpExtPort);
            var cred = new UserCredentials("admin", "changeit");

            var settings = EventStore.ClientAPI.ConnectionSettings.Create()
                .UseConsoleLogger()
                .KeepReconnecting()
                .SetDefaultUserCredentials(cred);

            var client = EventStoreConnection.Create(settings, endpoint);

            client.ConnectAsync().Wait();

            return client;
        }

        public IElasticClient ConfigureElastic()
        {
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

            var settings = new Nest.ConnectionSettings(node);
            settings.SetDefaultIndex(index);

            settings.SetDefaultTypeNameInferrer(type => type.FullName.Replace("Demo.Application.ServiceStack.", "").Replace('.', '_'));
            // Disable camel case field names (need to match out POCO field names)
            settings.SetDefaultPropertyNameInferrer(field => field);

            var client = new ElasticClient(settings);
            if (!client.IndexExists(index).Exists)
                client.CreateIndex(index, i => i
                        .Analysis(analysis => analysis
                            .TokenFilters(f => f
                                .Add("ngram", new Nest.NgramTokenFilter { MinGram = 2, MaxGram = 15 })
                                )
                            .Analyzers(a => a
                                .Add(
                                    "default_index",
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

            return client;
        }

        public override ServiceStackHost Init()
        {
            LogManager.LogFactory = new Log4NetFactory(true);
            NServiceBus.Logging.LogManager.Use<NServiceBus.Log4Net.Log4NetFactory>();

            var serverEvents = new MemoryServerEvents
            {
                IdleTimeout = TimeSpan.FromSeconds(30),
                NotifyChannelOfSubscriptions = false
            };

            var store = ConfigureStore();
            var elastic = ConfigureElastic();
            var eventstore = ConfigureEventStore();

            _container = new Container(x =>
            {
                x.For<IManager>().Use<Manager>();
                x.For<ICacheClient>().Use(new MemoryCacheClient());
                x.For<IServerEvents>().Use(serverEvents);
                x.For<ISubscriptionManager>().Use<MemorySubscriptionManager>();
                x.For<IDocumentStore>().Use(store).Singleton();
                x.For<IElasticClient>().Use(elastic).Singleton();
                x.For<IEventStoreConnection>().Use(eventstore).Singleton();
                x.For<IQueryProcessor>().Use<QueryProcessor>();
                x.For<IPersistCheckpoints>().Use<RavenCheckpointPersister>();

                x.Scan(y =>
                {
                    AllAssemblies.Matching("Application").ToList().ForEach(a => y.Assembly(a));

                    y.WithDefaultConventions();
                    y.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                    y.ConnectImplementationsToTypesClosing(typeof(IPagingQueryHandler<,>));
                });
            });

            var config = new BusConfiguration();
            //var conventions = config.Conventions();
            //conventions
            //    .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Events"))
            //    .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && t.Namespace.EndsWith("Commands"))
            //    .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Demo") && (t.Namespace.EndsWith("Messages") || t.Namespace.EndsWith("Queries")));

            config.LicensePath(@"C:\License.xml");

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            if (string.IsNullOrEmpty(endpoint))
                endpoint = "application.servicestack";

            config.EndpointName(endpoint);
            //config.AssembliesToScan(AllAssemblies.Matching("Presentation").And("Application").And("Domain").And("Library"));

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));
            config.UseSerialization<NServiceBus.JsonSerializer>();
            config.EnableInstallers();

            config.EnableFeature<Aggregates.EventStore>();
            config.EnableFeature<Aggregates.DurableConsumer>();

            var bus = Bus.Create(config).Start();

            _container.Configure(x => x.For<IBus>().Use(bus).Singleton());

            return base.Init();
        }

        public override void Configure(Funq.Container container)
        {
            JsConfig.IncludeNullValues = true;
            JsConfig.AlwaysUseUtc = true;
            JsConfig.TreatEnumAsInteger = true;

            SetConfig(new HostConfig { DebugMode = true });

            container.Adapter = new StructureMapContainerAdapter(_container);

            Plugins.Add(new SessionFeature());
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new RequestLogsFeature());

            Plugins.Add(new PostmanFeature
            {
                DefaultLabelFmt = new List<String> { "type: english", " ", "route" }
            });
            Plugins.Add(new CorsFeature(
                allowOriginWhitelist: new[] {
                    "http://localhost:9000"
                },
                allowCredentials: true,
                allowedHeaders: "Content-Type, Authorization",
                allowedMethods: "GET, POST, PUT, DELETE, OPTIONS"
                ));

            var appSettings = new AppSettings();

            Plugins.Add(new ValidationFeature());
            Plugins.Add(new ServerEventsFeature());
            Plugins.Add(new ServiceStack.Authentication.Plugin());
            Plugins.Add(new ServiceStack.Accounting.Plugin());
            Plugins.Add(new ServiceStack.Configuration.Plugin());

            //container.Register<IRedisClientsManager>(c =>
            //    new PooledRedisClientManager("localhost:6379"));
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
            //container.Register(c => c.Resolve<IRedisClientsManager>().GetClient());

            //container.Register<ICacheClient>(new MemoryCacheClient());

            //container.Register<IBus>(bus);
        }
    }
}