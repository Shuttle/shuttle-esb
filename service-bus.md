---
title: Shuttle.Esb API
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

| Command                     | Event                     |
| ---                        | ---                        |
| SendEMailCommand        | EMailSentEvent            |
| CreateCustomerCommand    | CustomerCreatedEvent    |
| PlaceOrderCommand        | OrderPlacedEvent        |
| CancelOrderCommand        | OrderCancelledEvent        |

## Message structure

Both **command** messages and **event** messages are basically data containers.  They are, therefore, data transfer objects and should not contain any behaviour.  If you would like to add some basic behaviour you should opt for extension methods as these will not interfere with serialization.  LINQ features, for example, may interfere with serialization.

Since the messages are plain classes you do not need to implement any specific interface or inherit from any specific class to make them work.

You may want to apply some convention to distinguish between the messages.  As indicated by the examples above a **Command** and **Event** suffix may be added to the **command** and **event** messages respectively.

In order to **send** a command or **publish** an event you need an instance of the [IMessageSender][MessageSender] interface.  This interface is implemented on both the `ServiceBus` class (via the `IServiceBus` interface) and the [HandlerContext].

# ServiceBusConfiguration

The `ServiceBusConfiguration` instance contains all the configuration required by the `ServiceBus` to operate and is built up from the application configuration file by default.  You can make use of your dependency injection container of choice to inject your own instance of `IServiceBusConfiguration` before creating the service bus in order to provide a custom configuration. 


[HandlerContext]: {{ "/handler-context" | relative_url }}
[MessageSender]: {{ "/message-sender" | relative_url }}
