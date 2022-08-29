# Worker Options

```c#
var configuration = 
    new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();

services.AddServiceBus(builder => 
{
    builder.Options.Worker = new WorkerOptions
    {
        DistributorControlInboxWorkQueueUri = 
            "queue://configuration/distributor-server-control-inbox-work",
        ThreadAvailableNotificationInterval = TimeSpan.FromSeconds(5)
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
      "Worker": {
        "DistributorControlInboxWorkQueueUri": 
            "queue://./distributor-server-control-inbox-work",
        "ThreadAvailableNotificationInterval": "00:00:05"
      }
    }
  }
}
```

## Options

| Options                            | Default         | Description    |
| ---                                | ---            | ---            |
| DistributorControlWorkQueueUri    |  | The control inbox work queue uri of the distributor endpoint that this endpoint can handle messages on behalf of. |
| ThreadAvailableNotificationInterval    | 15s    | A `TimeSpan` representing the duration to wait on an idle thread before notifying the distributor of availability *again*. |
