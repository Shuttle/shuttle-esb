---
title: Subscription Manager
layout: api
---
# ISubscriptionManager

An implementation of the `ISubscriptionManager` interface is used by endpoints to subscribe to message types and to get the endpoint uris that have subscribed to a message type.

There is no *default* implementation of the `ISubscriptionManager` interface as the data has to be persisted in some central data store.

Current implementations:

- [Sql Server]({{ site.baseurl }}/extensions-sql-server/index.html#SubscriptionManager)

## Methods

### Subscribe

~~~ c#
void Subscribe(IEnumerable<string> messageTypeFullNames)
void Subscribe(string messageTypeFullName)
void Subscribe(IEnumerable<Type> messageTypes)
void Subscribe(Type messageType)
void Subscribe<T>()
~~~

Attempts to subscribe the current endpoint inbox work queue uri to the list of message type full names.  If the subscription already exists then no action is taken.  If the subscription does *not* exist the subscription manager should attempt to make the entry and throw an exception should the endpoint not have permission to create the entry.

In a development environment the security around the subscriptions probably does not need to be too stringent but to prevent 'eavesdropping' in production environments that connection information used by the subscribed endpoints should be read-only.  Subscriptions should be managed more securely in a production environment and should rather be commissioned manually.

### GetSubscribedUris

~~~ c#
IEnumerable<string> GetSubscribedUris(object message)
~~~

Returns a list of endpoint uris that have subscribed to the type name of the given message.