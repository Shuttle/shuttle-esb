# Message Handler

In order to handle a message sent to an endpoint you need to implement the `IMessageHandler<>` interface on a class that is typically called the *message handler*.

## Properties

Although the interface does not have any properties you could implement the `IReusability` interface.  If this interface is *not* implemented the message handler is regarded as stateless and will be pooled.

### Reusability

By default all message handlers are regarded as stateless and the instances are re-used for subsequent calls.

If, however, you would like to mark any message handler as not being re-usable you can implement the `IReusability` interface and return `false` from the `IsReusable` property:

``` c#
bool IsReusable { get; }
```

Return `true` to indicate that the message handler instance can be re-used.  This usually results in faster performance since new message handlers do not have to be instantiated.  The performance gain will probably be negligible but stateless message handlers are preferred none-the-less.

## Methods

### ProcessMessage

``` c#
void ProcessMessage(IHandlerContext<T> context);
```

The `<T>` generic argument should be the type of the POCO message you are interested in.  This method will contain the actual implementation code that reacts to the message that is passed in.  If [Shuttle.Core.Transactions](https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-transactions.html) are enabled then this method will be wrapped in a `TransactionScope`.
