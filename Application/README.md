Application
===========

These projects represent objects that listen to events generated from the domain objects

The application projects build read models from the event stream and save them to theit db of choice.  In this example we use RavenDB.

Similar to the domain projects, each read model is getting its own folder.  These folders contain the Query commands the presentation layer can use to ask for data, 
as well as the messages we send in reply, and the query validators.  It also includes an EventHandler who handles events from the domain, and a QueryHandler who handles commands
from presentation.

To query application services, the presentation layer sends a command in the form of a query.  The application server responds to the command with a set of messages which are
the result of the query.
