# Message Handler Invoker

An implementation of the `IMessageHandlerInvoker` interface is used to invoke a mesage handler for the given message in the `PipelineEvent`.

If you do not specify your own implementation of the `IMessageHandlerInvoker` the default `MessageHandlerInvoker` will be used.  This invoker makes use of the `IServiceProvider` to create the required message handler.

## Methods

### Invoke

``` c#
MessageHandlerInvokeResult Invoke(PipelineEvent pipelineEvent);
```

Invoke the message handler using the data contained in the given `PipelineEvent`.

# MessageHandlerInvoker

Type `MessageHandlerInvoker` implements the `IMessageHandlerInvoker` interface and will attempt to find an implementation of the required `IMessageHandler<>` interface.

If no handler can be found the `MessageHandlerInvokeResult` return from the `Invoke` method will have an `Invoked` value of `false`.

A handler is created per thread and re-used.  Should you not want a handler to be re-used, or if you have some condition that determines re-use, you may implement the `IReusability` interface on the message handler and return the relevant `bool` value from the `IsReusable` property.