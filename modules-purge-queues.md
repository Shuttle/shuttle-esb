---
title: Purge Queues Module
layout: api
---
# Shuttle.Esb.Module.PurgeQueues

<div class="nuget-badge">
	<p>
		<code>Install-Package Shuttle.Esb.Module.PurgeQueues</code>
	</p>
</div>

The PurgeQueues module for Shuttle.Esb clears the specified queues on startup.

The module will attach the `PurgeQueuesObserver` to the `OnAfterInitializeQueueFactories` event of the `StartupPipeline` and purges the configured queues if the relevant queue implementation has implemented the `IPurgeQueue` interface.  If the relevant queue implementation has *not* implemented the `IPurgeQueue` interface only a warning is logged.

```xml
<configuration>
	<configSections>
		<section name="purgeQueues" type="Shuttle.Esb.Module.PurgeQueues.PurgeQueuesSection, Shuttle.Esb.Module.PurgeQueues"/>
	</configSections>

	<purgeQueues>
		<queues>
			<queue uri="msmq://./inbox" />
			<queue uri="sql://./inbox" />
		</queues>
	</purgeQueues>
</configuration>
```

The module will register itself using the [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping).