---
title: Msmq Extensions
layout: api
---
# MsmqQueue

All MSMQ queues are required to be **transactional**.  In addition to the actual queue a `msmq://host/queue$journal` queue will **always** be used.  If it does not exist it will be created, so if you are creating queues explicitly then remember to create these also.

MSMQ creates outgoing queues internally so it is not necessary to use an outbox.

## Installation / Activation

If you need to install / activate msmq you can [follow these instructions][msmq-installation-activation]

If you need to activate

## Configuration

The queue configuration is part of the specified uri, e.g.:

~~~xml
    <inbox
      workQueueUri="msmq://host/queue?useDeadLetterQueue=true"
	  .
	  .
	  .
    />
~~~

| Segment / Argument | Default	| Description | Version Introduced |
| --- | --- | --- | --- |
| useDeadLetterQueue	 | true | Specifies the value to pass to the 'UseDeadLetterQueue' property of the message sent. | 3.4.3 |

By default the `MsmqQueue` is a transactional queue that utilizes a journal queue when retrieving messages.  Please try not to change the default unless you have carefully considered your choice.  Although there is a slight performance penalty the defaults provide a relatively risk-free consumption of the queue.

In addition to this there is also a MSMQ specific section (defaults specified here):

~~~xml
<configuration>
  <configSections>
    <section name='msmq' type="Shuttle.ESB.MSMQ.MSMQSection, Shuttle.ESB.MSMQ"/>
  </configSections>
  
  <msmq
	localQueueTimeoutMilliseconds="0"
	remoteQueueTimeoutMilliseconds="2000"
  />
  .
  .
  .
<configuration>
~~~

[msmq-installation-activation]: {{ site.baseurl }}/msmq-installation-activation/index.html