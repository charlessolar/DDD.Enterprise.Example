# DDD.Enterprise.Example

**2016 Update**

The foundation of this solution is currently being used in my work - I am publishing an updated version almost a year after the initial Example repo because the foundation has changed a fair bit.

Unfortunently I do not have time to make a working sample project that runs and does something trivialy useful.  Therefore this repo should not be expected to run - its merely an *example* of how I structured a horizontally scalable application.

If you want to try out a solution that compiles and runs checkout a [previous commit](https://github.com/volak/DDD.Enterprise.Example/commit/5a644536eaf241dd255639d6fe6d60bf35ae87d0) But even that solution will be difficult to setup right.  This project has always been more to demonstrate structure than provide working code.  When dealing with distributed applications I'm not sure if any project would be easy to download and run.

**Original Readme**

An example architecture meant to be used for large deployments usable by enterprises.  Trivial examples from major projects leave some guesswork
on how to deploy for large organizations.  This solution demonstrates these concepts as applied for the enterprise.  

The architecture is designed for high-read low-write operations and follows DDD and CQRS standards for massive scale-out potential.

Projects used in this example:

- [NServiceBus](https://github.com/Particular/NServiceBus)
- [GetEventStore](https://github.com/EventStore/EventStore)
- [Aggregates.NET](https://github.com/volak/Aggregates.NET)
- [ServiceStack](https://github.com/ServiceStack/ServiceStack)
- [Riak](http://basho.com/products/#riak)
- [Elastic](https://www.elastic.co/)
- [RabbitMq](https://www.rabbitmq.com/)

**Architecture Overview**

Projects are organized according to the normal DDD layers.  You will find a Infrastructure, Domain, Application, and Presentation folders containing related projects.  The presentation layer is implemented as a WebApi meant to be used in conjuction with a web app or json client.

Commands and queries are sent mainly to servicestack's web api endpoints which publish messages on rabbitmq to be processed by the Domain in the case of commands, or the application in case of queries.  Commands are read into the domain handlers who read the aggregates and entities needed from Eventstore and otherwise process the command.  If accepted the entities produce events which are recorded to the event store.  The application projects, currently Riak and Elastic, listen for events from the store and do their own processing to build read models which servicestack queries for.



