---
title: Unity Extensions
layout: api
---
# UnityMessageHandlerFactory

The `UnityMessageHandlerFactory` inherits from the abstract `MessageHandlerFactory` class in order to implement the `IMessageHandlerFactory` interface.  This class will provide the message handlers from the `UnityContainer`.

~~~c#
	bus = ServiceBus
		.Create
		(
			c => c.MessageHandlerFactory(new UnityMessageHandlerFactory(new UnityContainer()))
		)
		.Start();
~~~
