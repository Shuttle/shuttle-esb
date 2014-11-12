---
title: Purge Inbox Module
layout: api
---
# Purge Inbox Module

The `PurgeInboxModule` may be found in the `Shuttle.ESB.Modules` assembly.  The module will attach the `PurgeInboxObserver` to the `OnAfterInitializeQueueFactories` event of the `StartupPipeline` and purges the inbox work queue if the relevant queue implementation has implemented the `IPurgeQueue` interface.  If it hasn't a warning is logged.

```c#
	var bus = ServiceBus
		.Create()
		.AddModule(new PurgeInboxModule())
		.Start();
```