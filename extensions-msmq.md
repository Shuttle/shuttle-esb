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
      workQueueUri="msmq://host/queue?transactional=false"
	  .
	  .
	  .
    />
```

If you do not specify the 'transactional' parameter it defaults to false.

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