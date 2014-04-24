---
title: MSMQ Extensions
layout: api
---
# MsmqQueue

MSMQ creates outgoing queues internally so it is not necessary to use an outbox.

## Configuration

The queue configuration is part of the specified uri, e.g.:

```xml
    <inbox
      workQueueUri="msmq://host/queue?journal-true&transactional=true"
	  .
	  .
	  .
    />
```

| Segment / Argument	| Default 	| Description |
| ---					| ---		| ---			|
| journal				| true		| Specifies whether a journal queue will be used when returning messages from the queue |
| transactional			| true		| Determines whether the queue is transactional or not |

So by default the `MsmqQueue` is a transactional queue that utilizes a journal queue when retrieving messages.  Please try not to change the default unless you have carefully considered your choice.  Although there is a slightt performance penalty the defaults provide a reletively risk-free consumption of the queue.

In addition to this there is also a MSMQ specific section (defaults specified here):

```xml
<configuration>
  <configSections>
    <section name='msmq' type="Shuttle.ESB.MSMQ.MSMQSection, Shuttle.ESB.MSMQ"/>
  </configSections>
  
  <rabbitmq
	localQueueTimeoutMilliseconds="0"
	remoteQueueTimeoutMilliseconds="2000"
  />
  .
  .
  .
<configuration>
```