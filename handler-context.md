---
title: HandlerContext
layout: api
---
# HandlerContext

The `HandlerContext` is a message-specific context that is passed to an [IMessageHandler] instance in order for the message handler to process the contained message.  

## Methods

The `HandlerContext` has an instance of a [MessageSender] and exposes the `IMessageSender` interface directly.

## Properties

### TransportMessage

``` c#
public TransportMessage TransportMessage { get; private set; }
```

Contains the actual [TransportMessage] that was received from the inbox work queue.

### Message

``` c#
public T Message { get; private set; }
```

The POCO message that is contained within the [TransportMessage] received.

### ActiveState

``` c#
public IThreadState ActiveState { get; private set; }
```

This can be used to determine wether the thread is still active.  If it is `Active` property returns `false` it indicates tha the endpoint is shutting down and the thread will be stopped as soon as all processing is completed.  If you handler is going to be performing lengthy operations, or many operations, it may be a good idea to cancel everything and throw an exception so thata retry will be attempted later; or simply send a copy of the message you are processing locally.

### Configuration

``` c#
public IServiceBusConfiguration Configuration { get; private set; }
```

Returns a reference to the [IServiceBusConfiguration].

[IMessageHandler]: {{ BASE_PATH }}/message-handler/index.html
[IServiceBusConfiguration]: {{ BASE_PATH }}/service-bus/index.html#ServiceBusConfiguration
[MessageSender]: {{ BASE_PATH }}/message-sender/index.html
