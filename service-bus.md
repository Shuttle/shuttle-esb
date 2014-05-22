---
title: Shuttle-ESB API
layout: api
---
# ServiceBus

There are basically three ways to communicate from one endpoint to another:

- sending **command** messages for processing
- sending **command** messages for *deferred* processing
- publishing **event** messages

A **command** message is only ever sent to **one** endpoint and at least **one** endpoint is required for the message to be successfully sent.  This requirement stems from a **command** being an instruction that will result in an action so it is something that is still going to take place.  If there is no endpoint to send a **command** to the instruction will not be executed whilst there is an expectation that the action would be executed.

An **event**, on the other hand, is something that has happened typically as a result of a **command** but not always.  There may be some other state that is noticed by the system that may require an **event** being published.  An event may have **zero** or **more** subscribers since there is no requirement for anyone to be interested in the event.  Typically when a business **event** is defined there should be at least one interested subscriber; else the **event** would not exist.  There may be some infrastructure-related eventst that may not always have a subscriber but it may be worthwhile publising the event anyway since it is something interesting that has occurred.

Some examples of *commands* and *events*:

| Command 					| Event 					|
| ---						| ---						|
| `SendEMailCommand`		| `EMailSentEvent`			|
| `CreateCustomerCommand`	| `CustomerCreatedEvent`	|
| `PlaceOrderCommand`		| `OrderPlacedEvent`		|
| `CancelOrderCommand`		| `OrderCancelledEvent`		|

## Message structure

Both **command** messages and **event** messages are basically data containers.  They are, therefore, data transfer objects and should not contain any behaviour.  If you would like to add some basic behaviour you should opt for extension methods as these will not interfere with serialization.  LINQ features, for example, may interfere with serialization.

Since the messages are plain classes you do not need to implement any specific interface or inherit from any specific class to make them work.

You may want to apply some convention to distinguish between the messages.  As indicated by the examples above a **Command** and **Event** suffix may be added to the **command** and **event** messages respectively.

## Sending **command** messages

Once you have an instance of the service bus available it is a matter of calling `Send` on the instance:

| Method												| Description							|
| ---													| ---									|
| `TransportMessage Send(object message)`				| This method is the most typical invocation and simply takes the message and sends it to the destination as determined by the `IMessageRouteProvider`. |
| `TransportMessage Send(object message, string uri)`	| Using this method a message can be sent to the specified `Uri`.  *More commonly used by Shuttle-ESB itself*. |
| `TransportMessage Send(object message, IQueue queue)`	| Should you have a specific `IQueue` instance available you could send the message directly there. *More commonly used by Shuttle-ESB itself*. |
| `TransportMessage SendLocal(object message)`			| The given message will be sent back to the inboc work queue of this endpoint. |
| `TransportMessage SendReply(object message)`			| This method is only available when handling a message and will send the given message back to the `RecipientInboxWorkQueueUri` of the `TransportMessage` being handled. |

## Sending deferred **command** messages

Sending deferred messages is functionally identical to sending message normally but the messages will only be processed once the given `DateTime` has been reached.  

When making use of deferred messages the endpoint that is responsible for processing the deferred message should ideally have `deferredQueueUri` configured for the inbox:

``` xml
	<serviceBus>
		<inbox
		  workQueueUri="msmq://./pubsub-client-inbox-work"
		  deferredQueueUri="msmq://./pubsub-client-inbox-deferred" <!-- THE DEFERRED QUEUE STORE -->
		  errorQueueUri="msmq://./shuttle-samples-error" />
	</serviceBus>
```

The processor on the deferred queue is optimized to process only when a message is due.  This leads to less IO than *not* having it.  If there is no deferred queue the message goes tot he work queue and will be ignored and may lead o sub-optimal processing of the work queue.

All you need to do now is call the relevant `SendDeferred` method on the available bus instance:

| Method																		| Description							|
| ---																			| ---									|
| `TransportMessage SendDeferred(DateTime at, object message)`					| This method is the most typical invocation and simply takes the message and sends it to the destination as determined by the `IMessageRouteProvider`. |
| `TransportMessage SendDeferred(DateTime at, object message, string uri)`		| Using this method a message can be sent to the specified `Uri`.  *More commonly used by Shuttle-ESB itself*. |
| `TransportMessage SendDeferred(DateTime at, object message, IQueue queue)`	| Should you have a specific `IQueue` instance available you could send the message directly there. *More commonly used by Shuttle-ESB itself*. |
| `TransportMessage SendDeferredLocal(DateTime at, object message)`				| The given message will be sent back to the inboc work queue of this endpoint. |
| `TransportMessage SendDeferredReply(DateTime at, object message)`				| This method is only available when handling a message and will send the given message back to the `RecipientInboxWorkQueueUri` of the `TransportMessage` being handled. |

## Publishing **event** messages

# ServiceBusConfiguration

TBD