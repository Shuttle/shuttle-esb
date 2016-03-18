---
title: [BREAKING CHANGE - HandlerContext]
layout: post
---

# HandlerContext

Release 5.0.0 of [shuttle-esb-core](https://github.com/Shuttle/shuttle-esb-core/releases/tag/v5.0.0) contains a ***breaking change***.

We have added an `IHandlerContext` interface to assist in testing handlers.

Previously you would implement an `IMessageHandler<Message>` as follows:

~~~ c#
	public void ProcessMessage(HandlerContext<Message> context) // <-- NOTE: HandlerContext
	{
		// your code goes here
	}
~~~

From this release goping forward you will need to implemented the message handlers using the `IHandlerContext` interface:

~~~ c#
	public void ProcessMessage(IHandlerContext<Message> context) // <-- NOTE: IHandlerContext
	{
		// your code goes here
	}
~~~

