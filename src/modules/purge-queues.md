# Purge Queues

```
PM> Install-Package Shuttle.Esb.Module.PurgeQueues
```

The PurgeQueues module for Shuttle.Esb clears the specified queues on startup.

The module will attach the `PurgeQueuesObserver` to the `OnAfterConfigure` event of the `StartupPipeline` and purges the configured queues if the relevant queue implementation has implemented the `IPurgeQueue` interface.  If the relevant queue implementation has *not* implemented the `IPurgeQueue` interface the purge is ignore.

## Configuration

```c#
services.AddPurgeQueuesModule(builder => 
{
	builder.Options.Uris = new List<string>
	{
		"scheme-a://configuration-name/queue-a",
		"scheme-b://configuration-name/queue-b"
	};
});
```

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "Modules": {
      "MessageForwarding": {
        "Uris": [
          "scheme-a://configuration-name/queue-a",
          "scheme-b://configuration-name/queue-b"
		]
	  }
	}
  }
}
```
