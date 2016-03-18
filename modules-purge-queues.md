---
title: Purge Queues Module
layout: api
---
# Purge Queues Module

The `PurgeQueuesModule` may be found in the `Shuttle.ESB.Modules` assembly.  The module will attach the `PurgeQueuesObserver` to the `OnAfterInitializeQueueFactories` event of the `StartupPipeline` and purges the queues configured in the `purgeQueues` configuration section:

~~~ xml
<configuration>
	<configSections>
		<section name="purgeQueues" type="Shuttle.ESB.Modules.PurgeQueuesSection, Shuttle.ESB.Modules"/>
	</configSections>

	<purgeQueues>
		<queues>
			<queue uri="msmq://./inbox" />
			<queue uri="sql://./inbox" />
		</queues>
	</purgeQueues>
</configuration>
~~~

The relevant queue implementation has to implement the `IPurgeQueue` interface.  If it doesn't a warning is logged.

~~~c#
	var bus = ServiceBus
		.Create.Create
		(
			c => c.AddModule(new PurgeQueuesModule())
		)
		.Start();
~~~