---
title: Msmq Extensions
layout: api
---
# MsmqQueue

```
PM> Install-Package Shuttle.Esb.Msmq
```

All Msmq queues are required to be **transactional**.  In addition to the actual queue a `msmq://host/queue$journal` queue will **always** be used.  If it does not exist it will be created, so if you are creating queues explicitly then remember to create these also.

Msmq creates outgoing queues internally so it is not necessary to use an outbox.

## Installation / Activation

If you need to install / activate msmq you can [follow these instructions][msmq-installation-activation].

## Configuration

The queue configuration is part of the specified uri, e.g.:

```xml
<inbox
    workQueueUri="msmq://host/queue?useDeadLetterQueue=true"
    .
    .
    .
/>
```

| Segment / Argument | Default    | Description | Version Introduced |
| --- | --- | --- | --- |
| useDeadLetterQueue     | true | Specifies the value to pass to the 'UseDeadLetterQueue' property of the message sent. | 3.4.3 |

By default the `MsmqQueue` is a transactional queue that utilizes a journal queue when retrieving messages.  Please try not to change the default unless you have carefully considered your choice.  Although there is a slight performance penalty the defaults provide a relatively risk-free consumption of the queue.

In addition to this there is also a Msmq specific section (defaults specified here):

```xml
<configuration>
  <configSections>
    <section name='msmq' type="Shuttle.Esb.Msmq.MsmqSection, Shuttle.Esb.Msmq"/>
  </configSections>
  
  <msmq
    localQueueTimeoutMilliseconds="0"
    remoteQueueTimeoutMilliseconds="2000"
  />
  .
  .
  .
<configuration>
```

A `MsmqConfiguration` instance implementing the `IMsmqConfiguration` interface will be registered using the [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping).  If you wish to override the configuration you should register your instance before calling the `ServiceBus.Register()` method.
