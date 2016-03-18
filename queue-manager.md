---
title: Queue Manager
layout: api
---
# IQueueManager

An implementation of the `IQueueManager` interface is used to manage the queues used in shuttle-esb.

The queue manager cannot be swapped out for your own implementation as it is integral to the functioning of shuttle-esb and the default implementation should suffice.

## Methods

### GetQueueFactory

~~~ c#
IQueueFactory GetQueueFactory(string scheme);
IQueueFactory GetQueueFactory(Uri uri);
~~~

The method will return an instance of the queue factory registered for the requested `scheme` in the uri.

### GetQueue

~~~ c#
IQueue GetQueue(string uri);
~~~

This method returns an `IQueue` implementation that interacts with the queuing mechanism represented by the `scheme` in the uri.  This method will attempt to return a cached `IQueue` instance.  If none is found a new instance is requested using the `CreateQueue` method.

Should the uri scheme be `resolver` this method will invoke the configured [UriResolver] to obtain the represented queue uri and wrap that as a `ResolvedQueue`.

### CreateQueue

~~~ c#
IQueue CreateQueue(string uri);
IQueue CreateQueue(Uri uri);
~~~

The method returns a new instance of the requested queue implementation represented by the uri scheme.

### CreatePhysicalQueues

~~~ c#
void CreatePhysicalQueues(IServiceBusConfiguration serviceBusConfiguration);
~~~

This method will attempt to create the physical queues configured for the inbox, outbox, control inbox, and deferred queues if they are present.  The relevant queue implementation is safe cast to an `ICreateQueue` instance and, if implemented, the queue creation will be attempted.

Please note that the creation may fail for a variety of reasons such as permissions or the relevant server cannot be reached.  For a production environment it is recommended that the required queues are manually created and assigned the relevant permissions.

### GetQueueFactories

~~~ c#
IEnumerable<IQueueFactory> GetQueueFactories();
~~~

Returns the `IQueueFactory` implementations that the queue manager is aware of.

### RegisterQueueFactory

~~~ c#
void RegisterQueueFactory(IQueueFactory queueFactory);
~~~

Use this method to explicitly register a queue factory instance.

### ContainsQueueFactory

~~~ c#
bool ContainsQueueFactory(string scheme);
~~~

This method determines whether the queue manager has a queue factory registered for the given scheme.

### UriResolver

~~~ c#
IUriResolver UriResolver { get; set; }
~~~

Use this property to get or set the relevant [UriResolver].

[UriResolver]: {{ site.baseurl }}/uri-resolver