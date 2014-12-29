# DDD.Enterprise.Example

An example architecture meant to be used for large deployments usable by enterprises.  Trivial examples from major projects leave some guesswork
on how to deploy for large organizations.  This solution demonstrates these concepts as applied for the enterprise.  

The architecture is designed for high-read low-write operations and follows DDD and CQRS standards for massive scale-out potential.

Projects used in this example:

- [NServiceBus](https://github.com/Particular/NServiceBus)
- [NEventStore](https://github.com/NEventStore/NEventStore)
- [Aggregates.NET](https://github.com/volak/Aggregates.NET)
- [ServiceStack](https://github.com/ServiceStack/ServiceStack)
- [RavenDB](https://github.com/ravendb/ravendb)
- [FluentValidation](https://github.com/JeremySkinner/FluentValidation)

**Architecture Overview**

There are currently 4 nservicebus endpoints.  One to send demo commands in DemoMessages, one to receive commands in Domain, an event handler in Application, and one for the presentation layer.

The Domain endpoint is configured to be a distributor to event listeners and Application is configured to subscribe.

In a production system you would configure a seperate distributor to distribute commands to your command handlers (domain projects), and another distributor to send events to event handlers (application projects).

You could even make seperate queues for different bounded contexts.

In theory you can have multiple different Domains and multiple Application handlers (and I would suggest you do)

**Presentation**

The presentation layer is currently a WIP.  In order to run it you will need redis running and use a REST client like POSTMAN to run commands against ServiceStack.

**Roadmap**

- Unit tests
- Simple HTML site
- Receiving events to the client

**Project References**

Its important to note that the Domain project is referencing all Domain.* projects, similarly the Application project is referencing all Application.* projects.
Each Domain.* and each Application.* project represents a bounded context.

**Notes**

Start visual studio as admin because NServicebus likes it that way

Configure the debugger to launch 3 projects, Domain, Application, and DemoMessages

Make sure NServiceBus creates all the MSMQ queues it needs


**Structure**

A couple things should be said about the structure of the solution.

The project mimics the standard DDD layers

- Presentation
- Application
- Domain
- Data Access
- Persistence

We are modeling the top 3 layers in this solution.  NEventStore and RavenDB handle Data Access and Persistence.
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

