---
title: Sql Server Extensions
layout: api
---
# SqlQueue

There is a `IQueue` implementation for Sql Server that enables a table-based queue.  Since this a table-based queue is not a _real_ queuing technology it is prudent to make use of a local outbox.

# SubscriptionManager

A Sql Server based `ISubscriptionManager` implementation is also provided.  The subscription manager caches all subscriptions forever so should a new subscriber be added be sure to restart the publisher endpoint service.

# DeferredMessageQueue

A `IDeferredMessageQueue` implementation is also available for Sql Server.