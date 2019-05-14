---
title: Uri Resolver
layout: api
---
# IUriResolver

An implementation of the `IUriResolver` interface is used by endpoints to subscribe to message types and to get the endpoint uris that have subscribed to a message type.

The configuration of the `IUriResolver` implementation to use is slightly different from most of the other components in shuttle in that it is more closely coupled to the [QueueManager] and, therefore, specified using the `UriResolver` property on the `QueueManager` instance.

## Methods

### Get

``` c#
Uri Get(string forUri);
```

Returns the resolved uri for the specified uri.  For instance, if the configured uri were `resolver://some-path` it may be mapped to real uri `rabbitmq://shuttle:shuttle!@localhost/my-queue`.

# DefaultUriResolver

The `DefaultUriResolver` makes use of the application configuration file to map the required uris:

``` xml
<uriResolver>
    <add name="resolver://host/queue-1" uri="msmq://./inbox-work-queue" />
    <add name="resolver://host/queue-2" uri="rabbitmq://user:password@the-server/inbox-work-queue" />
</uriResolver>
```

[QueueManager]: {{ "/queue-manager" | relative_url }}