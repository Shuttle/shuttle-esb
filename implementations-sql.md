---
title: Sql Server Extensions
layout: api
---
<a name="Queue"></a>

# Shuttle.Esb.Sql.Queue

<div class="nuget-badge">
	<p>
		<code>Install-Package Shuttle.Esb.Sql.Queue</code>
	</p>
</div>

Contains a sql-based `IQueue` implementation that enables a table-based queue.  Since a table-based queue is not a real queuing technology it is prudent to make use of a local outbox.

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

<a name="Subscription"></a>

# Shuttle.Esb.Sql.Subscription

<div class="nuget-badge">
	<p>
		<code>Install-Package Shuttle.Esb.Sql.Subscription</code>
	</p>
</div>

Contains a sql-based `ISubscriptionManager` implementation.  The subscription manager caches all subscriptions forever so should a new subscriber be added be sure to restart the publisher endpoint service.

There is also a subscription specific configuration section:

```xml
<configuration>
	<configSections>
		<section name="subscription" type="Shuttle.Esb.Sql.Subscription.SubscriptionSection, Shuttle.Esb.Sql.Subscription"/>
	</configSections>
  
	<subscription
		connectionStringName="Subscription"
		ignoreSubscribe="false"
	/>
  .
  .
  .
<configuration>
```

| Attribute | Default	| Description | Version Introduced |
| --- | --- | --- | --- |
| connectionStringName	 | Subscription | The name of the `connectionString` to use to connect to the subscription store. | |
| ignoreSubscribe			 | false		| If `true` the `ISubscriptionManager.Subscribe` method is ignored; else new subscription request are honored. | v6.0.9 |

Whenever the endpoint is configured as a worker no new subscriptions will be registered against the endpoint since any published events should be subscribed to only by the distributor endpoint.  However, there may be scenarios where one uses a broker, such as RabbitMQ, and there is no distributor endpoint since each endpoint can consume messages from the inbox queue.  In such situation one would either configure a specific endpoint as a subscriber where new subscriptions can be made, or configure the subscriptions manually using scripts or [Shuttle.Sentinel](https://shuttle.github.io/shuttle-sentinel/).

The `SubscriptionManager` will register itself using the [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping).

<a name="Idempotence"></a>

# Shuttle.Esb.Sql.Idempotence

<div class="nuget-badge">
	<p>
		<code>Install-Package Shuttle.Esb.Sql.Idempotence</code>
	</p>
</div>

Contains a sql-based `IIdempotenceService` implementation.  

There is also a subscription specific configuration section:

```xml
<configuration>
	<configSections>
		<section name="idempotence" type="Shuttle.Esb.Sql.Idempotence.IdempotenceSection, Shuttle.Esb.Sql.Idempotence"/>
	</configSections>
  
	<idempotence
		connectionStringName="Idempotence"
	/>
  .
  .
  .
<configuration>
```

| Attribute | Default	| Description | Version Introduced |
| --- | --- | --- | --- |
| connectionStringName	 | Idempotence | The name of the `connectionString` to use to connect to the idempotence store. | |

The `IdempotenceService` will register itself using the [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping).
