---
title: ESB Concepts
layout: api
---
# Why use a service bus?

A service bus provides a mechanism to decouple systems.  One system should not have any knowledge of the internal workings of another.  The way a service bus manages to do this is by using a messaging infrastructure in the form of queues.

You certainly can code directly against the messaging infrastructure and you will find that there are pros and cons w.r.t. each transport.  There are many decisions that you will need to make along the way and this is where a service bus will make your implementation somewhat easier.

Developing directly against the queuing system will inevitably lead to various abstractions that you will require to prevent duplication.

A service bus will provide opinions/implementations for:

- Message delivery:
    * exactly-once (distributed transactions - distributed transactions are not supported by all queuing systems)
    * at-least once (non-transactional)
- Retrying failed messages
- Request / Response
- Message distribution
- Publish/Subscribe (probably quite easy with RabbitMQ directly, not so much with MSMQ directly)
- Message idempotence
- Dependency injection
- Process management

A service bus will buy you quite a bit out-of=the-box whereas coding against the queues directly may be a bit of work to get going.

The following provides a quick overview of some service bus concepts as implemented in Shuttle.Esb that may help you along the way.

## Core

The basic parts of Shuttle.Esb consist of:

* Messages
* Queues
* Service bus

Every service bus instance is associated with, and therefore processes, only one input queue.  This is the inbox.  All messages received in the inbox are processed by the associated service bus instance.

## Messages

Messages are essentially data transfer objects that implement a specific message structure, e.g.:

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

Messages are processed by message handlers that are invoked by Shuttle.Esb.  When a service bus is started it starts listening for messages in an inbox queue.  Messages have to end up in the relevant queue to be processed.  The inbox configuration is specified in the application configuration file.

The approach taken is an **at-least-once** delivery mechanism.  This differs from an **exactly-once** delivery in that edge cases may result in a message being processed more than once (these should hardly ever occur).  For **exactly-once** delivery edge cases may result in message loss which is impossible to find since a duplicate message is easier to spot than no message at all.

It is important to note that all queues are non-destructive and should always be implemented with acknowledgement in mind.  As soon as a message is retrieved from the queue it should be possible to either acknowledge the message to release the message back onto the queue.

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

A service bus instance is created and started on application startup and disposed on exit.  A service bus can be hosted in any type of application but the most typical scenario is to host them as services.  Although you _can_ write your own service to host your service bus it is not a requirement since you may want to make use of the [generic service host]({{ "/generic-host/index.html" | relative_url }}).

# Message Types

## Command message

A command message is used to tell another system or component what to do.  This implies that the calling system is aware of the behaviour of the called system.  There is, therefore, a high degree of [behavioural coupling].

A command message always _**belongs**_ to a single endpoint.  Sending a command will never result in the message going to more than **one** queue.

### Starting a process

There are situations where we need to _start_ something off.  Let's take the case of receiving an order from a client.  In our application we would send a **CreateOrderCommand** to our order service.  This would kick off the relevant processs.

So from the client code:

``` c#
bus.Send(new CreateOrderCommand 
        {
            Name = "ClientName",
            Product = "ProductXYZ"
        });
```

The call would fail if there is nowhere to send the message.

We could publish an event such as **OrderReceivedEvent** and our order service could subscribe to the event.

```c#
bus.Publish(new OrderReceivedEvent
        {
            Name = "ClientName",
            Product = "ProductXYZ"
        });
```

The call would not fail should there be no subscribers.  

The difference lies purely in the intent of the message.  When we can use events they should be preferred but when certain actions are **required** in the system a command may be better suited.  Of course, even when using a command approach there may still be some other system interested in knowing that an order has been received and the order service would still publish the event.

### Lower-level functions (RPC)

In some situations an event will not be able to relay the intent of any particular action.  For instance, we may need to send an e-mail to a manager whenever an order is created (or when the total amount of the order exceeds a set limit).  The e-mail service responsible for sending e-mails via our smtp server will not be able to subscribe to the `OrderReceivedEvent` since the e-mail system would need to be adapted to accomodate rules from another system.

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

An event message is used to notify any subscribed components that something significant to the business has taken place.  Each endpoint subscribed to an event will get a copy of the event message.  If no subscribers exist for an event then publishing the event will have no effect.

## Document message

A document message is used to simply send data to an endpoint.  As with the event message it carries no intent and the recipient may do with it as it pleases.  This does not meean that data will be sent to an endpoint for no reason.  An endpoint may request a document from some service.  Or data is automatically sent to some endpoint since it may be a requirement.

_Document messages are not implemented in Shuttle.Esb._

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

