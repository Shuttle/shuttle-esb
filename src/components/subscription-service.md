# Subscription Service

An implementation of the `ISubscriptionService` interface is used by endpoints to subscribe to message types and to get the endpoint uris that have subscribed to a message type.

There is no *default* implementation of the `ISubscriptionService` interface as the data has to be persisted in some central data store.

## Methods

### GetSubscribedUris

``` c#
IEnumerable<string> GetSubscribedUris(object message)
```

Returns a list of endpoint uris that have subscribed to the type name of the given message.