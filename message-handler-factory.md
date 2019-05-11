---
title: Message Handler Factory
layout: api
---
# IMessageHandlerFactory

An implementation of the `IMessageHandlerFactory` interface is used to obtain an implementation of the `IMessageHandler` for the given message.  Once a message is received on the work queue and instance of the relevant `IMessageHandler` is required to process the message.  The actual message instance is passed to the factory to obtain the relevant `IMessageHandler` instance.

There is a `MessageHandlerFactory` abstract class that takes care of the pooling.  You may want to derive from this class when implementation a new message handler factory but it is entirely up to you.

The `DefaultMessageHandlerFactory` implementation of the `IMessageHandlerFactory` interface will locate all loaded types that derive from `IMessageHandler<>` and as long as the type has a **default constructor** the message handler type will be available for use.

If you are using a dependency injection container and you would like dependencies injected into your handler the you would need a `IMessageHandlerFactory` implementation that is aware of the DI container.  The `CastleMessageHandlerFactory` may serve as an example of how to achieve this.

## Methods

### GetHandler

``` c#
IMessageHandler GetHandler(object message);
```

The method will return a new or pooled [MessageHandler] for the message.

### ReleaseHandler

``` c#
void ReleaseHandler(IMessageHandler handler);
```

The method will return the [MessageHandler] to the pool if the message doesn't iomplement `IReusable` or the message handler's `IReusable.IsReusable` returns `true` the message handler will be pooled and re-used upon release. ; else it should be disposed of as necessary.

### MessageTypesHandled

``` c#
IEnumerable<Type> MessageTypesHandled { get; }
```

Returns a list of the message types that will be handled by the factory.

[MessageHandler]: {{ "/message-handler" | resolver_url }}
[TransportMessage]: {{ "/transport-message" | resolver_url }}