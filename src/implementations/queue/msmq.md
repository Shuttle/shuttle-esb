# MSMQ

```
PM> Install-Package Shuttle.Esb.Msmq
```

All MSMQ queues are required to be **transactional**.  In addition to the actual queue a `msmq://host/queue$journal` queue will **always** be used.  If it does not exist it will be created, so if you are creating queues explicitly then remember to create these also.

MSMQ creates outgoing queues internally so it is not necessary to use an outbox.

## Installation / Activation

You need to install / activate MSMQ on your system before using this queuing option.

## Configuration

Since an instance of the `IMsmqConfiguration` interface is required remember to register one.  Typically the default implementation will do:

``` c#
IComponentRegistry.Register<IMsmqConfiguration, MsmqConfiguration>();
```

The queue configuration is part of the specified uri, e.g.:

``` xml
    <inbox
      workQueueUri="msmq://host/queue?useDeadLetterQueue=true"
	  .
	  .
	  .
    />
``` 

| Segment / Argument | Default	| Description |
| --- | --- | --- | 
| useDeadLetterQueue	 | true | Specifies the value to pass to the 'UseDeadLetterQueue' property of the message sent. | 

By default the `MsmqQueue` is a transactional queue that utilizes a journal queue when retrieving messages.  Please try not to change the default unless you have carefully considered your choice.  Although there is a slight performance penalty the defaults provide a relatively risk-free consumption of the queue.

In addition to this there is also a Msmq specific section (defaults specified here):

``` xml
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