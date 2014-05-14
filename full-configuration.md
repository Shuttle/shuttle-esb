---
title: Full Configuration File
layout: api
---
# Full Application Configuration File

This configuration is the default application configuration file based version.  Some components that you may replace could very well have their own configuration stores.

To start off add the `Shuttle.ESB.Core.ServiceBusSection` configuration class from the `Shuttle.ESB.Core` assembly.

```xml
<configuration>
  <configSections>
    <section name='serviceBus' type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
  </configSections>
```

The most pertinent bit is the `serviceBus` tag.

```xml
  <serviceBus
    createQueues="true"  
    removeMessagesNotHandled="true"
    compressionAlgorithm=""
    encryptionAlgorithm="">
```

| Attribute						| Default 	| Description	|
| ---							| ---		| ---			|
| `createQueues`				| true		| The endpoint will attempt to create all local queues (inbox, outbox, control inbox) |
| `removeMessagesNotHandled`	| true		| indicates whether messages received on the endpoint that have no message handler should simply be removed (ignored).  If this attribute is `false` then the message will immmediately be placed in the error queue. |
| `compressionAlgorithm`		| empty	(no compression)	| The name of the compression algorithm to use when sending messages.  Out-of-the-box there is a GZip compression implementation (class `GZipCompressionAlgorithm` with name 'GZip'). |
| `encryptionAlgorithm`			| empty	(no entryption)		| The name of the encryption algorithm to use when sending messages.  Out-of-the-box there is a Triple DES implementation (class TripleDesEncryptionAlgorithm and name '3DES'). |

Use the `forwardingRoutes` tag to enable message forwarding.  All messages that are received and have been sucessfully handled will be forwarded to the specified queue.

```xml
    <forwardingRoutes>
      <messageRoute uri="msmq://./inbox">
        <add specification="StartsWith" value="Shuttle.Messages1" />
        <add specification="StartsWith" value="Shuttle.Messages2" />
      </messageRoute>
      <messageRoute uri="sql://./inbox">
        <add specification="TypeList" value="DoSomethingCommand" />
      </messageRoute>
    </forwardingRoutes>
```

The `messageRoutes` tag defines the routing for message that are sent using the `IServiceBus.Send` method.  You will notice that the structure is the same as the `forwardingRoutes` tag.

```xml
    <messageRoutes>
      <messageRoute uri="msmq://./inbox">
        <add specification="StartsWith" value="Shuttle.Messages1" />
        <add specification="StartsWith" value="Shuttle.Messages2" />
      </messageRoute>
      <messageRoute uri="sql://./inbox">
        <add specification="TypeList" value="DoSomethingCommand" />
      </messageRoute>
    </messageRoutes>
```

The `inbox` should be specified if the endpoint has message handlers that need to process incoming messages

```xml
    <inbox
      workQueueUri="msmq://./inbox-work"
      deferredQueueUri="msmq://./inbox-work-deferred"
      errorQueueUri="msmq://./shuttle-error"
      workQueueStartupAction="Purge"
      threadCount="25"
      durationToSleepWhenIdle="250ms,10s,30s"
      durationToIgnoreOnFailure="30m,1h"
      maximumFailureCount="25" 
      distribute="true|false" />
```

| Attribute						| Default 	| Description	|
| ---							| ---		| ---			|
| `workQueueStartupAction`		| 'None'	| <ul><li>QueueStartupAction.None - inbox work queue is left as is.</li><li>QueueStartupAction.Purge - inbox work queue is purged at start-up</li><ul> |
| `threadCount`					| 5			| The number of worker threads that will service the inbox work queue.  The deferred queue will always be serviced by only 1 thread. |
| `durationToSleepWhenIdle`		| 250ms\*4,500ms\*2,1s | |
| `durationToIgnoreOnFailure`	| 5m,10m,15m,30m,60m | |
| `maximumFailureCount`			| 5			| The maximum number of failures that are retried before the message is moved to the error queue. |
| `distribute`					| false		| If `true` the endpoint will act as only a distributor.  If `false` the endpoint will distribute messages if a worker is available; else process the message itself. |


For some queueing technologies the `outbox` may not be required.  Msmq, for instance, create its own outgoing queues.  However, it should be used in scenarios where you need a store-and-forward mechanism for sending messages when the underlying infrastructure does not provide this such as with a SqlServer table-based queue or maybe even the file system.  RabbitMQ will also need an outbox since the destination broker may not be available and it does not have the concept of outgoing queues.

```xml
    <outbox
      workQueueUri="msmq://./outbox-work"
      errorQueueUri="msmq://./shuttle-error"
      durationToSleepWhenIdle="250ms,10s,30s"
      durationToIgnoreOnFailure="30m,1h"
      maximumFailureCount="25"
      threadCount="5" />
```

| Attribute						| Default 	| Description	|
| ---							| ---		| ---			|
| `threadCount`					| 1			| The number of worker threads that will service the outbox work queue. |
| `durationToSleepWhenIdle`		| 250ms\*4,500ms\*2,1s | |
| `durationToIgnoreOnFailure`	| 5m,10m,15m,30m,60m | |
| `maximumFailureCount`			| 5			| The maximum number of failures that are retried before the message is moved to the error queue. |

You can also set the transaction scope behaviour by providing the `transactionScope` tag.  If you want your endpoint to be non-transactional then set the `enabled` attribute to `false`.  This will improve performance but messsage delivery and processing cannot be guaranteed.  If the underlying queueing infrastrcuture does not support 2-phase commit message delivery and processing also cannot be guaranteed.

```xml
    <transactionScope
      enabled="true"
      isolationLevel="ReadCommitted"
      timeoutSeconds="30" />
```

| Attribute				| Default 		| Description	|
| ---					| ---			| ---			|
| `enabled`				| true			| If `true` the message handling code in the receiving pipeline is wrapped in a `TransactionScope`. |
| `isolationLevel`		| ReadCommitted	| The transaction scope isolation level to use. |
| `timeoutSeconds`		| 30			| The number of seconds before a transaction scope times out. |

When the endpoint is not a physical endpoint but rather a worker use the `worker` tag to specify the relevant configuration.

```xml
    <worker
      distributorControlWorkQueueUri="msmq://./control-inbox-work"
      threadAvailableNotificationIntervalSeconds="5" />
```

| Attribute							| Default 		| Description	|
| ---								| ---			| ---			|
| `distributorControlWorkQueueUri`	| n/a			| The control work queue uri of the distributor endpoint that this endpoint can handle messages for. |
| `threadAvailableNotificationIntervalSeconds`	| 15	| The number of seconds to wait on an idle thread before notifying the distributor of availability *again* |

Since a worker sends thread availability to the physical distribution master the distributor needs to have a special inbox called the control inbox that is used for these notifications.

```xml
    <control
      workQueueUri="control-inbox-work"
      errorQueueUri="msmq://./shuttle-error"
      threadCount="25"
      durationToSleepWhenIdle="250ms,10s,30s"
      durationToIgnoreOnFailure="30m,1h"
      maximumFailureCount="25" />
```

| Attribute						| Default 	| Description	|
| ---							| ---		| ---			|
| `threadCount`					| 1			| The number of worker thread that will service the control work queue. |
| `durationToSleepWhenIdle`		| 250ms\*4,500ms\*2,1s | |
| `durationToIgnoreOnFailure`	| 5m,10m,15m,30m,60m | |
| `maximumFailureCount`			| 5			| The maximum number of failures that are retried before the message is moved to the error queue. |

Finally just close the relevant tags.

```xml
  </serviceBus>
</configuration>
```