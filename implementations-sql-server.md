---
title: Sql Server Extensions
layout: api
---
# SqlQueue

There is a `IQueue` implementation for Sql Server that enables a table-based queue.  Since this a table-based queue is not a real queuing technology it is prudent to make use of a local outbox.

## Configuration

The queue configuration is part of the specified uri, e.g.:

~~~xml
    <inbox
      workQueueUri="sql://connectionstring-name/table-queue"
	  .
	  .
	  .
    />
~~~

In addition to this there is also a Sql Server specific section (defaults specified here):

~~~xml
<configuration>
  <configSections>
    <section name='sqlServer' type="Shuttle.Esb.SqlServer.SqlServerSection, Shuttle.Esb.SqlServer"/>
  </configSections>
  
  <sqlServer
	subscriptionManagerConnectionStringName="Subscription"
	idempotenceServiceConnectionStringName="Idempotence"
	scriptFolder=""
	ignoreSubscribe="false"
  />
  .
  .
  .
<configuration>
~~~

| Attribute | Default	| Description | Version Introduced |
| --- | --- | --- | --- |
| subscriptionManagerConnectionStringName	 | Subscription | The name of the `connectionString` to use to connect to the subscription store. | |
| idempotenceServiceConnectionStringName		 | Idempotence	| The name of the `connectionString` that stores the idempotence data. | |
| scriptFolder				 | (empty)	| A folder containing any scripts that can be used to override default behaviour by specifying individual scripts that perform the relevant queries.  If empty the [default queries] are used. | |
| ignoreSubscribe			 | false		| If `true` the `ISubscriptionManager.Subscribe` method is ignored; else new subscription request are honored. | v6.0.9 |

Whenever the endpoint is configured as a worker no new subscriptions will be registered against the endpoint since any published events should be subscribed to only by the distributor endpoint.  However, there may be scenarios where one uses a broker, such as RabbitMQ, and there is no distributor endpoint since each endpoint can consume messages from the inbox queue.  In such situation one would either configure a specific endpoint as a subscriber where new subscriptions can be made, or configure the subscriptions manually using scripts or [Shuttle.Sentinel](https://shuttle.github.io/shuttle-sentinel/).

# SubscriptionManager

A Sql Server based `ISubscriptionManager` implementation is also provided.  The subscription manager caches all subscriptions forever so should a new subscriber be added be sure to restart the publisher endpoint service.

# IdempotenceService

A `IIdempotenceService` implementation is also available for Sql Server.