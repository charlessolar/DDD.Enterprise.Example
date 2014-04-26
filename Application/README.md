Application
===========

These projects represent objects that listen to events generated from the domain objects

Models are read models for this bounded context, query is a folder for query request handlers.  Eg. Get 8 items
Queries should be requested via nservicebus request/response

THis project is configured to save read models to RavenDb, however you could literally do anything inside a worker.  Store in memory, or use any other storage (including sql), you could even publish events to a SignalR hub so connected clients get updates in real time