---
title: Architecture & Patterns
layout: api
---
# Concepts

Code samples provided on this page do not represent a sample or solution but do show how some of the concepts would be applied in Shuttle ESB.  For help on putting together your first implementation you can take a look at the [getting started]({{ site.baseurl }}/getting-started/index.html) page.

The basic parts of Shuttle ESB consist of:

* Messages
* Queues
* Service bus

Every service bus instance is associated with, and therefore processes, only one input queue.  This is the inbox.  All messages received in the inbox are processed by the associated service bus instance.

## Messages

Shuttle is based on messages.  The messages are data transfer objects that implement a specific message interface, e.g.:

``` c#
    public class ActivateMemberCommand
    {
        string MemberId { get; set; }
    }

    public class MemberActivatedEvent
    {
        string MemberId { get; set; }
    }
```

## Queues

Messages are processed by message handlers that are invoked by Shuttle ESB.  When a service bus is started it starts listening for messages in an inbox queue.  So messages have to end up in the relevant queue to be processed.  The inbox configuration is specified in the application configuration file.

The approach taken is an **at-least-once** delivery.  This differs from an **exactly-once** delivery in that edge cases may result in a message being processed more than once (these should hardly ever occur).  However, for other mechanism edge cases may result in a message loss which is impossible to spot (a duplicate message is easier to spot than no message at all).

It is important to note that all queues are non-destructive and should always be implemented with acknowledgement in mind.  So as soon as a message is retrieved from the queue it should be possible to either acknowledge the message to release the message back onto the queue.

## Service bus

A service bus instance is required in every application that accesses the service bus.  To configure the service bus a combination of code, the application configuration file, and custom components is used, e.g.:

``` c#
    public class ServiceBusHost : IHost, IDisposable
    {
        private static IServiceBus bus;

        public void Start()
        {
            bus = ServiceBus.Create().Start();
        }

        public void Dispose()
        {
            bus.Dispose();
        }
    }
```

A service bus instance is created and started on application startup and disposed on exit.  A service bus can be hosted in any type of application but the most typical scenario is to host them as services.  Although you _can_ write your own service to host your service bus it is not a requirement since you may want to make use of the [generic service host]({{ site.baseurl }}/generic-host/index.html).

# Message Types

## Command message

A command message is used to tell another system or component what to do.  This implies that the calling system is aware of the behaviour of the called system.  There is, therefore, a high degree of [behavioural coupling].

A command message always _**belongs**_ to a single endpoint.  Sending a command will never result in the message going to more than **one** queue.

Although one should aim to minimize these message types they are required in certain situations as will be discussed in the following sections.

### Starting a process

There are situations where we need to _start_ something off.  Let's take the case of receiving an order from a client.  In our application we would send a **CreateOrderCommand** to our order service.  This would kick off the relevant processs.

So from the client code:

``` c#
    bus.Send(new CreateOrderCommand("ClientName", "ProductXYZ"));
```

The call would fail if there is nowhere to send the message.

Now we could publish an event such as **OrderReceivedEvent** and our order service could subscribe to the event and also kick everything off.

```c#
    bus.Publish(new OrderOrderReceivedEvent("ClientName", "ProductXYZ"));
```

The call would not fail should there be no subscribers.  

So the difference is purely the intent of the message.  When we can use events they should be preferred but when certain actions are **required** in the system a command may be better suited.  Of course, even when using a command approach there may still be some other system interested in knowing that an order has been received so the order service would then publish the event.

### Lower-level functions (RPC)

In some situations an event will not be able to relay the intent of any particular action.  For instance, we may need to send an e-mail to a manager whenever an order is created (or when the total amount of the order exceeds a set limit).  The e-mail service responsible for sending e-mails via our smtp server will not be able to subscribe to the OrderReceivedEvent since the e-mail system would need to be adapted to accomodate rules from another system.

In this case the e-mail system is responsible for sending e-mails.  Any system that would like to send a mail will need to decide when to do so.  Therefore, the order service would send a *command* to the e-mail service:

```c#
    bus.Send(new SendMailCommand
                 {
                     To = "manager@ordercompany.co.za",
                     From = "orderservice@ordercompany.co.za",
                     Subject = "Important Order Received",
                     Body = "Order Details"
                 });
```

## Event message

An event message is used to notify any subscribed components that something significant to the business has taken place.  Each component subscribed to an event will get a copy of the event message.  If no subscribers exist for an event then publishing the event will have no effect.

## Document message

A document message is used to simply send data to an endpoint.  As with the event message it carries no intent and the recipient may do with it as it pleases.  This does not meean that data will be sent to an endpoint for no reason.  An endpoint may request a document from some service.  Or data is automatically sent to some endpoint since it may be a requirement.

# Coupling

## Behavioural coupling

Behavioural coupling refers to THE degree one system is aware of the behaviour of another.  When a command is sent to a system you expect it to behave in a particular way.  This represents a high degree of behavioural coupling.  When an event message is published there is no expectation from any subscriber to react in any specific way.  This is a low degree of behavioural coupling.

It is conceivable that there may be an expectation that an event will result in a particular outcome in which case the behavioural coupling increases again.

## Temporal coupling

Temporal coupling refers to the availability of services **when** they are required.  

Should *ServiceA* require *ServiceB* to be available for *ServiceA* to function there is a high degree of temporal coupling.  Conversely, if *ServiceA* can continue to operate even though *ServiceB* is down then there is a low degree of temporal coupling.

A synchronous web-service call is an example of high temporal coupling.

Now you may be wondering how *ServiceA* can continue to operate even though it requires *ServiceB* to perform some function.  The answer is asynchronous communication using queues.  One may argue that a web-service call may be made asynchronously but there is a difference.  *ServiceA* may go down before a required response is received resulting in the web-service call failing.

With messages things always move in one direction at a time.  *ServiceA* to *ServiceB* is one operation and will eventually complete.  *ServiceB* to *ServiceA* is another movement and will eventually complete.

# Patterns

## Request / Response

For some background on **Request/Response** messaging pattern you can have a look at the [Wikipedia article](http://en.wikipedia.org/wiki/Request-response).

![Request/Response Image]({{ site.baseurl }}/assets/images/Architecture-RequestResponse.png "Request/Response")

To request an endpoint to perform a certain function you send a command message:

```c#
    bus.Send(new RequestMessage());
```

Although this is a very simple pattern it results in rather tight behavioural coupling.  This is not necessarily a bad thing and in many instances it is definitely required.

Typically the message handler for the command message goes about its business and processes the message.  But there will be times when a response is required.

The response can then be a command message or an event message and you can simply call the **reply** method on the service bus instance:

```c#
    bus.Send(new ResponseMessage(), c => c.Reply());
```

The response may, of course, be decoupled by publishing an event message but it is up to the implementor to decide the mechanism.  This would then no longer be request/response but rather publish/subscribe.  The advantage of request/response isthat it provides the ability to respond to the caller directly whereas publishing a message would result in **all** publishers receiving a copy of the message.

## Publish / Subscribe

For some background on **Publish/Subscribe** messaging pattern you can have a look at the [Wikipedia article](http://en.wikipedia.org/wiki/Publish/subscribe).

![Publish/Subscribe Image]({{ site.baseurl }}/assets/images/Architecture-PublishSubscribe.png "Publish/Subscribe")

This pattern results in no behavioural coupling between the publisher and subscriber(s).  In fact, there may be no subscribers to a particular event message whatsoever but that would not be a typical scenario as an event should be published for some business reason and _this_ implies that there should be _at least_ one subscriber.  To publish an event message you use the following:

```c#
    bus.Publish(new EventMessage());
```

Each subscriber receives its own copy of the message to process.  This differs substantially from message distribution where a particular message will only be sent to a single worker to handle.

## Message Distribution

It is conceivable that an endpoint can start falling behind with its processing if it receives too much work.  In such cases it may be changed to distribute messages to worker nodes.

![Message Distribution Image]({{ site.baseurl }}/assets/images/Architecture-MessageDistribution.png "Message Distribution")

An endpoint will automatically distribute messages to workers if it receives a worker availability message.  An endpoint can be configured to only distribute messages, and not process, by setting the `distribute` attribute of the `inbox` configuration tag to `true`.

Since message distribution is integrated into the inbox processing the same endpoint simply needs to be installed aa many times as required on different machines as workers.  The endpoint that you would like to have messages distributed on would require a control inbox configuration since all Shuttle messages should be processed without waiting in a queue like the inbox proper behind potentially thousands of messages.  Each worker is identified as such in its configuration and the control inbox of the endpoint performing the distribution is required:

```xml
<configuration>
   <configSections>
      <section name="serviceBus" type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
   </configSections>

   <serviceBus>
      <control 
		  workQueueUri="msmq://./control-inbox-work" 
		  errorQueueUri="msmq://./shuttle-error"/>
      <inbox 
		  distribute="true"
		  workQueueUri="msmq://./inbox-work" 
		  errorQueueUri="msmq://./shuttle-error"/>
   </serviceBus>
</configuration>
```

Any endpoint that receives messages can be configured to include message distribution.

You then install as many workers as you require on as many machines as you want to and configure them to talk to a distributor.  The physical distributor along with all the related workers form the logical endpoint for a message.  The worker configuration is as follows:

```xml
<configuration>
   <configSections>
      <section name="serviceBus" type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
   </configSections>

   <serviceBus>
      <worker
         distributorControlWorkQueueUri="msmq:///control-inbox=work" />
      <inbox
         workQueueUri="msmq://./workerN-inbox-work"
         errorQueueUri="msmq://./shuttle-error"
         threadCount="15">
      </inbox>
   </serviceBus>
</configuration>
```

As soon as the application configuration file contains the **worker** tag each thread that goes idle will send a message to the distributor to indicate that a thread has become available to perform word.  The distributor will then send a message for each available thread.

### Message Distribution Exceptions

Some queueing technologies do not require message distribution.  Instead of a worker another instance of the endpoint can consume the same input queue.  This mechanism applies to brokers.  Since brokers manage queues centrally the messages are consumed via consumers typically running per thread.  Where the consumers originates does not matter so the queue can be consumed from various processes.

The broker style differes from something like Msmq or Sql-based queues where the message-handling is managed by the process hosting the thread-consumers.  Here `process-A` would not be aware of which messages are being consumed by `process-B` leading to one *stealing* messages from the other.

# Message Routing

Typically when sending a message the message is a command.  It does not _have_ to be a command and you _can_ send an event message to a specific endpoint but more-often-than-not you will be sending a command.  Messages are sent by calling one of the relevant overloaded methods on the service bus instance:

```c#
		TransportMessage Send(object message);
		TransportMessage Send(object message, Action<TransportMessageConfigurator> configure);
```

Only messages that have not `RecipientInboxWorkQueueUri` set will be routed by the service bus.

The `TransportMessage` envelope will be returned if you need access to any of the metadata available for the message.

Shuttle ESB uses an implementation of a `IMessageRouteProvider` to determine where messages are sent.

```c#
	public interface IMessageRouteProvider
	{
		IEnumerable<string> GetRouteUris(object message);	
	}
```

The message route provider to use is specified when constructing the service bus:

```
	bus = ServiceBus
		.Create(c => c.MessageRouteProvider(new DefaultForwardingRouteProvider())
		.Start();
```

The `DefaultMessageRouteProvider` makes use of the application configuration file to determine where to send messages:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
   <configSections>
      <section name="serviceBus" type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
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
```

Each implementation of `IMessageRouteProvider` can determine the routes however it needs to from the given message.  A typical scenario, and the way the `DefaultMessageRouteProvider` works, is to use the full type name to determine the destination.

**Please note**: each message type may only be sent to _one_ endpoint (using `Send`).
