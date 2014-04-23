---
title: RabbitMQ Extensions
layout: api
---
# RabbitMQQueue

RabbitMQ does not provide 2-phase commit out-of-the-box.  Although implementing it is not too much effort the 2PC adds tremendous overhead (as it does for anything).  For this reason we do not use 2PC with RabbitMQ.

Instead we rely on the [idempotence service]({{ site.baseurl }}/idempotence-service/index.html).

Since RabbitMQ talks directly to a queue on any server it is suggested that you use an outbox that specifies a local queue just in case the remote queue is not immediately available.

## Configuration

The queue configuration is part of the specified uri, e.g.:

```xml
    <inbox
      workQueueUri="rabbitmq://username:password@host:port/virtualhost/queue?prefetchCount=25"
	  .
	  .
	  .
    />
```


| Tables        | Are           | Cool  |
| ------------- |:-------------:| -----:|
| col 3 is      | right-aligned | $1600 |
| col 2 is      | centered      |   $12 |
| zebra stripes | are neat      |    $1 |

| Segment / Argument	| Default	| Description |
| --------------------- | ---------	| ----------- |		
| username:password		| empty		| 	|
| virtualhost			| /			|	|
| port					| default	|	|
| journal				| true		| Specifies whether a journal queue will be used when returning messages from the queue |
| transactional			| true		| Determines whether the queue is transactional or not |

The `user information` and `virtual host` bits are optional.

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
