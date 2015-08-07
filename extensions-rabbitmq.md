---
title: RabbitMQ Extensions
layout: api
---
# RabbitMQQueue

RabbitMQ does not provide 2-phase commit out-of-the-box.  Although implementing it is not too much effort the 2PC adds tremendous overhead (as it does for anything).  For this reason shuttle does not use 2PC with RabbitMQ.

Instead we rely on the [idempotence service]({{ site.baseurl }}/idempotence-service/index.html).

Since RabbitMQ talks directly to a queue on any server it is suggested that you use an outbox that specifies a local queue just in case the remote queue is not immediately available.

## Installation

If you need to install RabbitMQ you can <a target='_blank' href='https://www.rabbitmq.com/install-windows.html'>follow these instructions</a>.

## Configuration

The queue configuration is part of the specified uri, e.g.:

```xml
    <inbox
      workQueueUri="rabbitmq://username:password@host:port/virtualhost/queue?prefetchCount=25&amp;durable=true"
	  .
	  .
	  .
    />
```

| Segment / Argument | Default	| Description | Version Introduced |
| --- | --- | --- | --- |
| username:password	 | empty|	| |
| virtualhost		 | /	|	| |
| port				 | default	|	| |
| prefetchcount			 | 25		| Specifies the number of messages to prefetch from the queue. | |
| durable			 | true     | Determines whether messages will be persisted.  Note: be very mindful of the possible consequences before setting to 'false'. | v3.5.0 |

In addition to this there is also a RabbitMQ specific section (defaults specified here):

```xml
<configuration>
  <configSections>
    <section name='rabbitmq' type="Shuttle.ESB.RabbitMQ.RabbitMQSection, Shuttle.ESB.RabbitMQ"/>
  </configSections>
  
  <rabbitmq
	localQueueTimeoutMilliseconds="250"
	remoteQueueTimeoutMilliseconds="2000"
	connectionCloseTimeoutMilliseconds="1000"
	requestedHeartbeat="30"
	operationRetryCount="3"
  />
  .
  .
  .
<configuration>
```
