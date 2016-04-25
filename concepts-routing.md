---
title: Message Routing Concepts
layout: api
---
# Message Routing

Once you have instantiated a message you need to get it to a specific endpoint.  You can let Shuttle.Esb decide this for you implicitly by configuring a routing mechanism or you can even specify the endpoint explicitly.

Typically when sending a message that message is a command.  It does not _have_ to be a command and you _can_ send an event message to a specific endpoint but more-often-than-not you will be sending a command.  Messages are sent by calling one of the relevant overloaded methods on the service bus instance:

~~~c#
		TransportMessage Send(object message);
		TransportMessage Send(object message, Action<TransportMessageConfigurator> configure);
~~~

Only messages that have no `RecipientInboxWorkQueueUri` set will be routed by the service bus.

The `TransportMessage` envelope will be returned should you need access to any of the metadata available for the message.

Shuttle.Esb uses an implementation of an `IMessageRouteProvider` to determine where messages are sent.

~~~c#
	public interface IMessageRouteProvider
	{
		IEnumerable<string> GetRouteUris(object message);	
	}
~~~

The message route provider to use is specified when constructing the service bus:

~~~
	bus = ServiceBus
		.Create(c => c.MessageRouteProvider(new DefaultMessageRouteProvider())
		.Start();
~~~

**Please note**: you not have to explicitly configure the `DefaultMessageRouteProvider` as it is instatiated for you.  Only specify the `IMessageRouteProvider` if you need to use an instance other than the default.

The `DefaultMessageRouteProvider` makes use of the application configuration file to determine where to send messages:

~~~xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
   <configSections>
      <section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
   </configSections>

   <serviceBus>
      <messageRoutes>
         <messageRoute uri="msmq://serverA/inbox">
            <add specification="StartsWith" value="Shuttle.Messages1" />
            <add specification="StartsWith" value="Shuttle.Messages2" />
         </messageRoute>
         <messageRoute uri="sql://serverB/inbox">
            <add specification="TypeList" value="DoSomethingCommand, Assembly" />
         </messageRoute>
         <messageRoute uri="msmq://serverC/inbox">
            <add specification="Regex" value=".+[Cc]ommand.+" />
         </messageRoute>
         <messageRoute uri="sql://serverD/inbox">
            <add specification="Assembly" value="TheAssemblyName" />
         </messageRoute>
      </messageRoutes>
   </serviceBus>
</configuration>
~~~

Each implementation of `IMessageRouteProvider` can determine the routes however it needs to from the given message.  A typical scenario, and the way the `DefaultMessageRouteProvider` works, is to use the full type name to determine the destination.

**Please note**: each message type may only be sent to _one_ endpoint (using `Send`).
