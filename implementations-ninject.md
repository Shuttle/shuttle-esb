---
title: Ninject Extensions
layout: api
---
# NinjectMessageHandlerFactory

The `NinjectMessageHandlerFactory` inherits from the abstract `MessageHandlerFactory` class in order to implement the `IMessageHandlerFactory` interface.  This class will provide the message handlers from the `NinjectContainer`.

~~~c#
	bus = ServiceBus
		.Create
		(
			c => c.MessageHandlerFactory(new NinjectMessageHandlerFactory(new StandardKernel()))
		)
		.Start();
~~~
