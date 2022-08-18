# Queue Service

An implementation of the `IQueueService` interface is used to manage the queues used in Shuttle.Esb.

The queue service should not be swapped out for your own implementation as it is integral to the functioning of Shuttle.Esb and the default implementation should suffice.

## Methods

### Get

``` c#
IQueue Get(string uri);
```

This method returns an `IQueue` implementation that interacts with the queuing mechanism represented by the `scheme` in the uri.  This method will attempt to return a cached `IQueue` instance.  If none is found a new instance is requested using the `IQueueFactoryService.Create()` method.

Should the uri scheme be `resolver` this method will invoke the configured `IUriResolver` to obtain the represented queue uri and wrap that as a `ResolvedQueue`.

### Contains

``` c#
public bool Contains(string uri);
```

This method determines whether the queue service has a queue registered for the given URI.
