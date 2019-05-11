---
title: Purge Inbox Module
layout: api
---
# Shuttle.Esb.Module.PurgeInbox

```
PM> Install-Package Shuttle.Esb.Module.PurgeInbox
```

The PurgeInbox module for Shuttle.Esb clears the inbox work queue upon startup.

The module will attach the `PurgeInboxObserver` to the `OnAfterInitializeQueueFactories` event of the `StartupPipeline` and purges the inbox work queue if the relevant queue implementation has implemented the `IPurgeQueue` interface.  If the inbox work queue implementation has *not* implemented the `IPurgeQueue` interface only a warning is logged.

The module will register itself using the [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping).