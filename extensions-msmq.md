---
title: MSMQ Extensions
layout: api
---
# MsmqQueue

Since the MSMQ `IQueue` implementation is a _real_ queuing technology it is not necessary to use a local outbox.  You also get full 2-phase commit (2PC) functionality.

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

|Segment / Argument	|Default|Description|
|journal			|true	|Specifies whether a journal queue will be used when returning messages from the queue |
|transactional		|true	|Determines whether the queue is transactional or not |

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