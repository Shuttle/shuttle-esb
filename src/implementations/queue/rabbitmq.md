# RabbitMQ

```
PM> Install-Package Shuttle.Esb.RabbitMQ
```

This RabbitMQ implementation follows the `at-least-once` delivery mechanism supported by Shuttle.Esb and since RabbitMQ is a broker all communication takes place immediately with the broker.

If necessary you may want to use an *outbox* for a `store-and-forward` solution.  By using a transactional outbox such as the sql implementation you could roll back sending of messages on failure.

## Installation

If you need to install RabbitMQ you can <a target='_blank' href='https://www.rabbitmq.com/install-windows.html'>follow these instructions</a>.

## Configuration

Since an instance of the `IRabbitMQConfiguration` interface is required remember to register one.  Typically the default implementation will do:

``` c#
IComponentRegistry.Register<IRabbitMQConfiguration, RabbitMQConfiguration>();
```

The queue configuration is part of the specified uri, e.g.:

``` xml
<inbox
    workQueueUri="rabbitmq://username:password@host:port/virtualhost/queue?prefetchCount=25&amp;durable=true&amp;persistent=true"
    .
    .
    .
/>
```

| Segment / Argument | Default    | Description | 
| --- | --- | --- | 
| username:password     | empty|    |
| virtualhost         | /    |    |
| port                 | default    |    |
| prefetchcount             | 25        | Specifies the number of messages to prefetch from the queue. |
| durable             | true     | Determines whether the queue is durable.  Note: be very mindful of the possible consequences before setting to 'false'. |
| persistent             | true     | Determines whether messages will be persisted.  Note: be very mindful of the possible consequences before setting to 'false'. |
| priority             | empty     | Determines the number of priorities supported by the queue. |

In addition to this there is also a RabbitMQ specific section (defaults specified here):

``` xml
<configuration>
  <configSections>
    <section name='rabbitmq' type="Shuttle.Esb.RabbitMQ.RabbitMQSection, Shuttle.Esb.RabbitMQ"/>
  </configSections>
  
  <rabbitmq
    localQueueTimeoutMilliseconds="250"
    remoteQueueTimeoutMilliseconds="2000"
    connectionCloseTimeoutMilliseconds="1000"
    requestedHeartbeat="30"
    operationRetryCount="3"
    useBackgroundThreadsForIO="true"
  />
  .
  .
  .
<configuration>
```
