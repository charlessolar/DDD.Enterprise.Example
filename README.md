NES.RavenDB.Example
===================

An example of getting NES configured and running with NServicebus, RavenDB, and NEventStore

Follows DDD, CQRS, EventSourcing patterns

I spent several hours of my life having to dig deep into these projects to figure this out - passing on the info to those with less time of their hands.


**Architecture Overview**

There are currently 3 nservicebus endpoints.  One to send demo commands in SendMessages, one to receive commands in Domain and an event handler in Application.

The Domain endpoint is configured to be a distributor to event listeners and Application is configured to subscribe.

In a production system you would configure a seperate distributor to distribute commands to your command handlers (domain projects), and another distributor to send events to event handlers (application projects).

You could even make seperate queues for different bounded contexts.

In theory you can have multiple different Domains and multiple Application handlers (and I would suggest you do)

**Project References**

Its important to note that the Domain project is referencing all Domain.* projects, similarly the Application project is referencing all Application.* projects.
Each Domain.* and Application.* project represents a bounded context.

**Notes**

Start visual studio as admin because NServicebus likes it that way

Configure the debugger to launch 3 projects, Domain, Application, and DemoMessages

Run each first by itself so NServicebus can create the MSMQ queues


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


