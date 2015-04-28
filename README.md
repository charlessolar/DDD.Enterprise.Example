# DDD.Enterprise.Example

An example architecture meant to be used for large deployments usable by enterprises.  Trivial examples from major projects leave some guesswork
on how to deploy for large organizations.  This solution demonstrates these concepts as applied for the enterprise.  

The architecture is designed for high-read low-write operations and follows DDD and CQRS standards for massive scale-out potential.

Projects used in this example:

- [NServiceBus](https://github.com/Particular/NServiceBus)
- [GetEventStore](https://github.com/EventStore/EventStore)
- [Aggregates.NET](https://github.com/volak/Aggregates.NET)
- [ServiceStack](https://github.com/ServiceStack/ServiceStack)
- [RavenDB](https://github.com/ravendb/ravendb)
- [FluentValidation](https://github.com/JeremySkinner/FluentValidation)

**Architecture Overview**

Projects are organized according to the normal DDD layers.  You will find a Infrastructure, Domain, and Application folders containing related projects.  The presentation layer is not implemented at this time, queries and commands can be run against the Servicestack endpoint using Postman for now.

Currently the Servicestack endpoint runs most of the show.  It sends commands out via NServicebus and connects to both RavenDB and ElasticSearch to run queries.  At the moment it also inserts data into both DBs via events received from NSB.  This process will be migrated to Aggregates.Net consumers in the GetEventStore fashion.

Commands sent from Servicestack will be received to the domain project, which writes events to GetEventStore.  Subscribers to the event store will receive these event updates as intended.

**Features**

- An example of a query processor from various examples online
- A caching application layer
- Sending SSE events when data models are updated
- Various data models to help bootstrap (base query, paging query, etc)
- Seeding application with a well thoughtout design
- Automatic Development / Staging / Production solution app configs

**How to Use**

You will need a NServicebus license, install it to C:\License.xml  (demo licenses work)

Configure visual studio to launch 3 projects, Domain, Application, and Seed by right clicking the solution and going to Properties->Startup project
You may need to run as administrator so NServicebus can create MSMQ queues

Once started, select the seed project and press any key to start seeding data

You can then use Postman to query servicestack urls


**Structure**

A couple things should be said about the structure of the solution.

The project mimics the standard DDD layers

- Presentation
- Application
- Domain
- Data Access
- Persistence
- Infrastructure

We are modeling just the application and domain layers in this solution.  GetEventStore handles Data Access and Persistence.
In each folder you will find 1 'master' project and a child.  IE

> Domain/

>> Domain.csproj

>> Domain.Inventory.csproj

The purpose of the Domain project is simply to be an NServicebus endpoint.  You can configure your own endpoints however you like.

The Domain.* projects should represent your bounded contexts.  Same with the Application.* projects.

Inside the domain projects I create a folder for every aggregate root.  Some projects create a root Events and Commands folder - 
however for many aggregate roots this easily gets out of hand.

Each folder has a sub folder for Events and Commands specific to the aggregate root.  Also inside the folder is the aggregate root command handler 
and the aggregate root itself.

I decided on this structure because it keeps things very nicely organized - how you do it is up to you.

For the application projects, I simply have a Handlers folder and a Models folder to model all the read models for that bounded context.

For larger deployments I am sure that structure will change to group read models somehow.

## Troubleshooting

If you have issues getting started, there are a few things to check.  Firstly, make sure you install NServiceBus from their [web site](http://particular.net/).  The installer will install RavenDB and MSMQ for you.

Next, make sure you run Visual Studio as Administrator, and that you run each endpoint as admin seperately.  This can be done by going to the build bin folder, or by using the debugger to run them yourself.  This must be done because programs started from VS without the debugger will not be admin, so they won't be able to create their queues.

After successfully starting each project your queues should look like this 

![Queue Config](/doc/private_queues.png?raw=true)

You will also need to edit the solution's properties to launch all 3 endpoints at once.  

![Solution properties](/doc/solution_properties.png?raw=true)

