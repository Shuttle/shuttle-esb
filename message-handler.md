---
title: IMessageHandler
layout: api
---
# IMessageHandler

In order to handle a message sent to an endpoint you need to implement the `IMessageHandler<>` interface on a class that is typically called the *message handler.

## Properties

### IsReusable

``` c#
bool IsReusable { get; }
```

This property is defined on the base interface `IMessageHandler`.  Return `true` from to indicate that the message handler instance can be re-used.  This usually results in faster performance since new message handler do not have to be created by the [IMessageHandlerFactory].  The performance gain will probably be negligible but stateless message handlers are preferred none-the-less.

## Methods

### ProcessMessage

``` c#
void ProcessMessage(HandlerContext<T> context);
```

The `<T>` generic argument should be the type of the POCO message you are interested in.  This method will contain the actuall implementation code that reacts to the message that is passed in.  If handler transport scope is enabled then this method will be wrapped in a `TransactionScope`:

```xml
	<serviceBus
		<transactionScope
			enabled="true"
			isolationLevel="ReadCommitted"
			timeoutSeconds="30" />
	</serviceBus>
```

[HandlerContext]: {{ BASE_PATH }}/handler-context/index.html
[IMessageHandlerFactory]: {{ BASE_PATH }}/message-handler-factory/index.html
