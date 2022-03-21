# Purge Inbox

```
PM> Install-Package Shuttle.Esb.Module.PurgeInbox
```

The PurgeInbox module for Shuttle.Esb clears the inbox work queue upon startup.

The module will attach the `PurgeInboxObserver` to the `OnAfterInitializeQueueFactories` event of the `StartupPipeline` and purges the inbox work queue if the relevant queue implementation has implemented the `IPurgeQueue` interface.  If the inbox work queue implementation has *not* implemented the `IPurgeQueue` interface only a warning is logged.

## Registration / Activation

The required components may be registered by calling `ComponentRegistryExtensions.RegisterPurgeInbox(IComponentRegistry)`.

In order for the module to attach to the `IPipelineFactory` you would need to resolve it using `IComponentResolver.Resolve<PurgeInboxModule>()`.
