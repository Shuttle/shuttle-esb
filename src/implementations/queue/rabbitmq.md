# RabbitMQ

```
PM> Install-Package Shuttle.Esb.RabbitMQ
```

This RabbitMQ implementation follows the `at-least-once` delivery mechanism supported by Shuttle.Esb.

If necessary you may want to use an *outbox* for a `store-and-forward` solution.  By using a transactional outbox such as the `Shuttle.Esb.Sql.Queue` implementation you could roll back sending of messages on failure.

## Installation

If you need to install RabbitMQ you can <a target='_blank' href='https://www.rabbitmq.com/download.html'>follow these instructions</a>.

## Configuration

The URI structure is `rabbitmq://configuration-name/queue-name`.

```c#
services.AddRabbitMQ(builder =>
{
    builder.AddOptions("local", new RabbitMQOptions
    {
        Host = "localhost",
        VirtualHost = "/",
        Port = -1,
        Username = "shuttle",
        Password = "shuttle!",
        PrefetchCount = 25,
        QueueTimeout = TimeSpan.FromMilliseconds(25),
        RequestedHeartbeat = TimeSpan.FromSeconds(30),
        ConnectionCloseTimeout = TimeSpan.FromSeconds(1),
        OperationRetryCount = 3,
        UseBackgroundThreadsForIO = true,
        Priority = 0,
        Persistent = true,
        Durable = true
    });
});
```

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "RabbitMQ": {
      "local": {
        "Host": "localhost",
        "VirtualHost": "/",
        "Port": -1,
        "Username": "shuttle",
        "Password": "shuttle!",
        "PrefetchCount": 25,
        "QueueTimeout": "00:00:25",
        "RequestedHeartbeat": "00:00:30",
        "ConnectionCloseTimeout": "00:00:01",
        "OperationRetryCount": 3,
        "UseBackgroundThreadsForIO": true,
        "Priority": 0,
        "Persistent": true,
        "Durable": true
      }
    }
  }
}
```

## Options

| Option | Default    | Description | 
| --- | --- | --- |
| `Host` | | The RabbitMQ host to connect to. |
| `VirtualHost` | `"/"` | The virtual host to connect to. |
| `Port` | -1 | Specifies the port to connect to.  A value of `-1` represents `AmqpTcpEndpoint.UseDefaultPort`. |
| `Username` | | The username to send as a credential. |
| `Password` | | The password to send as a credential. |
| `PrefetchCount` | 25 | Specifies the number of messages to prefetch from the queue. |
| `QueueTimeout` | `00:00:25` | How long to wait when retrieving a message from the queue before timing out and returing `null`. |
| `RequestedHeartbeat` | `00:00:30` | Heartbeat timeout to use when negotiating with the server. |
| `ConnectionCloseTimeout` | `00:00:01` | The duration to wait wait for connections to be closed. |
| `OperationRetryCount` | 3 | How many times to retry relevant queue operations in the event that they fail.  Once the retries have run out the original exception is thrown. |
| `UseBackgroundThreadsForIO` | `true` | Determines whether backgrounds threads are used for the I/O loop. |
| `Priority` | 0 | Determines the number of priorities (`x-max-priority`) supported by the queue. |
| `Persistent` | true | Determines whether messages will be persisted.  Please be sure of the possible consequences before setting to 'false'. |
| `Durable` | true | Determines whether the queue is durable.  Please be sure of the possible consequences before setting to 'false'. |