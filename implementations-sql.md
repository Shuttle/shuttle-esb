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

# Supported providers

Currently only the `System.Data.SqlClient` provider name is supported but this can easily be extended.  Feel free to give it a bash and please send a pull request if you *do* go this route.  You are welcome to create an issue and assistance will be provided where able.

## Configuration

The configuration section is optional as the defaults will be applied when no section is provided.

```xml
<configuration>
	<configSections>
		<section name="subscription" type="Shuttle.Esb.Sql.Subscription.SubscriptionSection, Shuttle.Esb.Sql.Subscription"/>
	</configSections>
  
	<subscription
		connectionStringName="Subscription"
		subscribe="Normal|Ensure|Ignore"
	/>
  .
  .
  .
<configuration>
```

| Attribute | Default	| Description | Version Introduced |
| --- | --- | --- | --- |
| `connectionStringName`	 | Subscription | The name of the `connectionString` to use to connect to the subscription store. | |
| `subscribe`	| Normal | Indicates how calls to the `Subscribe` method are dealt with: `Normal` is the ***default*** and will subscribe to the given message type(s) if they have not been subscribed to yet.  `Ensure` will check to see that the subscription exists and if not will throw an `ApplicationException`.  `Ignore` will simply ignore the subscription request.
| <strike>ignoreSubscribe</strike>			 | false		| *Obsolete*: use the `subscribe` option. | v6.0.9 |

Whenever the endpoint is configured as a worker no new subscriptions will be registered against the endpoint since any published events should be subscribed to only by the distributor endpoint.  When using a broker such as RabbitMQ all the endpoints feed off the same work queue uri and any of the endpoints could create the subscription.

When moving to a non-development environment it is recommended that you make use of the `Ensure` option for the `subscribe` attribute since any change to the work queue uri will result in possible duplicate subscriptions.  

For any environment you could manually configure subscription using either scripts or or [Shuttle.Sentinel](https://shuttle.github.io/shuttle-sentinel/) once it becomes feasible.

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
