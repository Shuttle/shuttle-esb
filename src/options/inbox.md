# Inbox Options

```c#
var configuration = 
    new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();

services.AddServiceBus(builder => 
{
    builder.Options.Inbox = new InboxOptions
    {
        WorkQueueUri = "queue://configuration/inbox-work",
        DeferredQueueUri = "queue://configuration/inbox-deferred",
        ErrorQueueUri = "queue://configuration/inbox-error",
        ThreadCount = 25,
        DurationToSleepWhenIdle = new List<TimeSpan>
        {
            TimeSpan.FromMilliseconds(250),
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(30)
        },
        DurationToIgnoreOnFailure = new List<TimeSpan>
        {
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(30)
        },
        MaximumFailureCount = 2,
        Distribute = false,
        DistributeSendCount = 5
    };
    
    // or bind from configuration
    configuration
        .GetSection(ServiceBusOptions.SectionName)
        .Bind(builder.Options);
})
```

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "Inbox": {
        "WorkQueueUri": "queue://configuration/inbox-work",
        "DeferredQueueUri": "queue://configuration/inbox-deferred",
        "ErrorQueueUri": "queue://configuration/inbox-error",
        "ThreadCount": 25,
        "DurationToSleepWhenIdle": [
          "00:00:00.250",
          "00:00:10",
          "00:00:30"
        ],
        "DurationToIgnoreOnFailure": [
          "00:30:00",
          "01:00:00"
        ],
        "MaximumFailureCount": 25,
        "Distribute": false,
        "DistributeSendCount": 5
      }
    }
  }
}
```

## Options

| Option                        | Default     | Description    |
| ---                            | ---        | ---            |
| ThreadCount                    | 5            | The number of worker threads that will service the work queue.  The deferred queue will always be serviced by only 1 thread. |
| DurationToSleepWhenIdle        | 250ms\*4,500ms\*2,1s,5s | A list of `TimeSpan` instances.  Each successive idle processing run will move to the next entry in the list; resets as soon as a message is processed. |
| DurationToIgnoreOnFailure    | 30s,2m,5m | A list of `TimeSpan` instances.  Each failure will move to the next entry.|
| MaximumFailureCount            | 5            | The maximum number of failures that are retried before the message is moved to the error queue if there is one and the queue is not a stream; else the message is released.   |
| Distribute                    | false        | If `true` the endpoint will act as only a distributor.  If `false` the endpoint will distribute messages if a worker is available; else process the message itself. |
| DistributeSendCount | 5 | The number of messages to send to the worker per available thread message received.  If less than 1 the default will be used.  |
