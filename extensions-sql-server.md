---
title: Sql Server Extensions
layout: api
---
# SqlQueue

There is a `IQueue` implementation for Sql Server that enables a table-based queue.  Since this a table-based queue is not a real queuing technology it is prudent to make use of a local outbox.

## Configuration

The queue configuration is part of the specified uri, e.g.:

```xml
    <inbox
      workQueueUri="sql://connectionstring-name/table-queue"
	  .
	  .
	  .
    />
```

In addition to this there is also a RabbitMQ specific section (defaults specified here):

```xml
<configuration>
  <configSections>
    <section name='sqlServer' type="Shuttle.ESB.SqlServer.SqlServerSection, Shuttle.ESB.SqlServer"/>
  </configSections>
  
  <sqlServer
	subscriptionManagerConnectionStringName="Subscription"
	scriptFolder=""
  />
  .
  .
  .
<configuration>
```

# SubscriptionManager

A Sql Server based `ISubscriptionManager` implementation is also provided.  The subscription manager caches all subscriptions forever so should a new subscriber be added be sure to restart the publisher endpoint service.

# IdempotenceService

A `IIdempotenceService` implementation is also available for Sql Server.