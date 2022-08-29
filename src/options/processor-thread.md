# Processor Thread Options

```c#
services.AddServiceBus(builder => 
{
    builder.Options.ProcessorThread = new ProcessorThreadOptions
    {
        JoinTimeout = TimeSpan.FromSeconds(15),
        IsBackground = false,
        Priority = ThreadPriority.Normal
    };
})
```

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "ProcessorThread": {
        "JoinTimeout": "00:00:15",
        "IsBackground": false,
        "Priority": "Lowest"  
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
