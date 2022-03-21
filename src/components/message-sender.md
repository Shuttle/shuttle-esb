# Message Sender

The purpose of the `IMessageSender` is to abstract sending and publishing capabilities.  The `MessageSender` class provides the actual implementation and both the `ServiceBus` and `HandlerContext` classes hold a reference to a `MessageSender`.

## Methods

### CreateTransportMessage

``` c#
TransportMessage CreateTransportMessage(object message, Action<TransportMessageConfigurator> configure);
```

Returns a new instance of a `TransportMessage` using the `TransportMessageConfigurator` provided by the `configure` action by invoking the `TransportMessagePipeline`.

### Dispatch

``` c#
public void Dispatch(TransportMessage transportMessage);
```

This method invokes the `DispatchTransportMessagePipeline` to have the given `TransportMessage` eventually enqueued on the target queue as specified by the `RecipientInboxWorkQueueUri` of the `TransportMessage`.

### Send

``` c#
public TransportMessage Send(object message);
```

Creates and then dispatches a `TransportMessage` using the message routing as configured.  The newly instantiated `TransportMessage` is returned.

``` c#
public TransportMessage Send(object message, Action<TransportMessageConfigurator> configure)
```

Creates and then dispatches a `TransportMessage` using the `TransportMessageConfigurator` returned by the `configure` action.  The newly instantiated `TransportMessage` is returned.

### Publish

``` c#
public IEnumerable<TransportMessage> Publish(object message)
```

Creates and then dispatches a `TransportMessage` for each uri returned by the registered `ISubscriptionManager` instance.  The newly instantiated `TransportMessage` collection returned with one message for each of relevant `RecipientInboxWorkQueueUri` subscription Uris.

``` c#
public IEnumerable<TransportMessage> Publish(object message, Action<TransportMessageConfigurator> configure)
```

Creates and then dispatches a `TransportMessage` for each uri returned by the registered `ISubscriptionManager` instance.  There should be very few instances where this method will be required.  The newly instantiated `TransportMessage` collection returned with one message for each of relevant `RecipientInboxWorkQueueUri` subscription Uris.
