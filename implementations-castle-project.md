---
title: Castle Project Extensions
layout: api
---
# CastleMessageHandlerFactory

The `CastleMessageHandlerFactory` inherits from the abstract `MessageHandlerFactory` class in order to implement the `IMessageHandlerFactory` interface.  This class will provide the message handlers from the `WindsorContainer`.

```c#
	bus = ServiceBus
		.Create
		(
			c => c.MessageHandlerFactory(new CastleMessageHandlerFactory(new WindsorContainer()))
		)
		.Start();
```

## Note on dependency injection

The `DefaultMessageHandlerFactory` registers all `IMessageHandler<>` implementations in the current `AppDomain`.  As soon as you use a container this responsibility falls on the implementer.

The message distribution makes use of `IMessageHandler<>` implementations in the core and there may be one or more modules, if used, that have message handlers.

You can use the `RegisterHandlers` method of the `CastleMessageHandlerFactory` instance to perform this registration for you:

```c#
	bus = ServiceBus
		.Create
		(
			c => .MessageHandlerFactory(new CastleMessageHandlerFactory(new WindsorContainer()).RegisterHandlers())
		)
		.Start();
```

You can also pass a specific `Assembly` to the `RegisterHandlers` method to register only message handlers in the specified assembly.