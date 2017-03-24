---
title: Message Handler Invoker
layout: api
---
# IMessageHandlerInvoker

An implementation of the `IMessageHandlerInvoker` interface is used to invoke a mesage handler for the given message in the `PipelineEvent`.

If you do not specify your own implementation of the `IMessageHandlerInvoker` the `DefaultMessageHandlerInvoker` will be used.  This invoker makes use of the `IMessageHandlerFactory` to create the required message handler factory.

## Methods

### Invoke

``` c#
MessageHandlerInvokeResult Invoke(PipelineEvent pipelineEvent);
```

Invoke the message handler using the data contained in the given `PipelineEvent`.