---
title: Castle Project Extensions
layout: api
---
# CastleMessageHandlerFactory

The `CastleMessageHandlerFactory` inherits from the abstract `MessageHandlerFactory` class in order to implement the `IMessageHandlerFactory` interface.  This class will provide the message handlers from the `WindsorContainer`.

```c#
	bus = ServiceBus
		.Create()
		.MessageHandlerFactory(new CastleMessageHandlerFactory())
		.Start();
```