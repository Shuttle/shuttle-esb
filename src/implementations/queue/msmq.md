# MSMQ

```
PM> Install-Package Shuttle.Esb.Msmq
```

All MSMQ queues are required to be **transactional**.  In addition to the actual queue a `msmq://configuration-name/queue-name$journal` queue will **always** be used.  If it does not exist it will be created, so if you are creating queues explicitly then remember to create these also.

MSMQ creates outgoing queues internally so it is not necessary to use an outbox.

## MSMQ Installation / Activation

You need to install / activate MSMQ on your system before using this queuing option.

## Configuration

The URI structure is `msmq://configuration-name/queue-name`.

```c#
services.AddMsmq(builder =>
{
    builder.AddOptions("local", new MsmqOptions
    {
        Path = ".\private$", // for local queues
        Path = "FormatName:DIRECT=TCP:127.0.0.1\private$", // for IP addresses
        Path = "FormatName:DIRECT=OS:{host-name}\private$",
        Timeout = Timespan.Zero,
        UseDeadLetterQueue = false
    });
});
```

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "Msmq": {
      "Timeout": "00:00:02",
      "UseDeadLetterQueue": false,
      "Path": "some-path" 
    }
  }
}
``` 

## Options

| Option | Default	| Description |
| --- | --- | --- | 
| `Path` | | The [MessageQueue.Path](https://docs.microsoft.com/en-us/dotnet/api/system.messaging.messagequeue.path?view=netframework-4.8) to use to connect to the queue. |
| `UseDeadLetterQueue` | `true` | Specifies the value to pass to the 'UseDeadLetterQueue' property of the message sent. | 
| `Timeout` | 00:00:00 | Timespan indicating how long to wait for queue operations to complete. |
