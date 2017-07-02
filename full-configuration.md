---
title: Full Configuration File
layout: api
---
# Full Application Configuration File

This configuration is the default application configuration file based version.  Some components that you may replace could very well have their own configuration stores.

To start off add the `Shuttle.Esb.ServiceBusSection` configuration class from the `Shuttle.Esb` assembly.

```xml
<configuration>
  <configSections>
    <section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
  </configSections>
```

It is also possible to group the Shuttle configuration in a `shuttle` group:

```xml
<configuration>
	<configSections>
		<sectionGroup name="shuttle">
			<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
		</sectionGroup>
	</configSections>
```

The most pertinent bit is the `serviceBus` tag.

```xml
  <serviceBus
    cacheIdentity="true"  
    createQueues="true"  
    removeMessagesNotHandled="false"
    compressionAlgorithm=""
    encryptionAlgorithm="">
```

| Attribute						| Default 	| Description	| Version Introduced |
| ---							| ---		| ---			| --- |
| `registerHandlers`				| true		| Will call the `RegisterHandlers` method on the `IMessageHandlerFactory` implementation if set to `true`. | [v7.0.0](https://github.com/Shuttle/Shuttle.Esb/releases/tag/v7.0.0) |
| `cacheIdentity`				| true		| Determines whether or not to re-use the identity returned by the `IIdentityProvider`. | [v6.2.0](https://github.com/Shuttle/Shuttle.Esb/releases/tag/v6.2.0) |
| `createQueues`				| true		| The endpoint will attempt to create all local queues (inbox, outbox, control inbox) | |
| `removeMessagesNotHandled`	| false		| indicates whether messages received on the endpoint that have no message handler should simply be removed (ignored).  If this attribute is `true` the message will simply be acknowledged; else the message will immmediately be placed in the error queue.  *The default changed from **true** to **false** in v7.0.1*. | |
| `compressionAlgorithm`		| empty	(no compression)	| The name of the compression algorithm to use when sending messages.  Out-of-the-box there is a GZip compression implementation (class `GZipCompressionAlgorithm` with name 'GZip'). | |
| `encryptionAlgorithm`			| empty	(no entryption)		| The name of the encryption algorithm to use when sending messages.  Out-of-the-box there is a Triple DES implementation (class TripleDesEncryptionAlgorithm and name '3DES'). | |



The `IIdentityProvider` implementation is responsible for honouring the `cacheIdentity` attribute.

Use the `queueFactories` tag to configure how you would like to locate queue factories.  By default the current `AppDomain` is scanned for implementations of `IQueueFactory` along with all assemblies in the base directory (recursively).  These queue factories have to have a parameterless constructor in order to be instantiated.

```xml
	<queueFactories scan="true|false">
		<add type="Shuttle.Esb.Msmq.MsmqQueueFactory, Shuttle.Esb.Msmq" />
		<add type="Shuttle.Esb.RabbitMQ.RabbitMQQueueFactory, Shuttle.Esb.RabbitMQ" />
		<add type="Shuttle.Esb.SqlServer.SqlQueueFactory, Shuttle.Esb.SqlServer" />
	</queueFactories>
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

The `inbox` should be specified if the endpoint has message handlers that need to process incoming messages.

```xml
    <inbox
      workQueueUri="msmq://./inbox-work"
      deferredQueueUri="msmq://./inbox-work-deferred"
      errorQueueUri="msmq://./shuttle-error"
      threadCount="25"
      durationToSleepWhenIdle="250ms,500ms,1s,5s"
      durationToIgnoreOnFailure="30m,1h"
      maximumFailureCount="25" 
      distribute="true|false" 
      distributeSendCount="5" />
```

| Attribute						| Default 	| Description	|
| ---							| ---		| ---			|
| `threadCount`					| 5			| The number of worker threads that will service the inbox work queue.  The deferred queue will always be serviced by only 1 thread. |
| `durationToSleepWhenIdle`		| 250ms,500ms,1s,5s | |
| `durationToIgnoreOnFailure`	| 5m,30m,60m | |
| `maximumFailureCount`			| 5			| The maximum number of failures that are retried before the message is moved to the error queue. |
| `distribute`					| false		| If `true` the endpoint will act as only a distributor.  If `false` the endpoint will distribute messages if a worker is available; else process the message itself. |
| `distributeSendCount` | 5 | The number of messages to send to the work per available thread message received.  If less than 1 the default will be used.  |


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

Use the `modules` tag to configure modules that can be loaded at runtime.  These modules have to have a parameterless constructor in order to be instantiated; else add them programmatically if you need to specify parameters.

```xml
	<modules>
		<add type="Shuttle.Esb.Modules.ActiveTimeRangeModule, Shuttle.Esb.Modules" />
	</modules>
```

If you need to make use of the `DefaultUriResolver` you can specify the mappings as follows:

```xml
	<uriResolver>
		<add name="resolver://host/queue-1" uri="msmq://./inbox-work-queue" />
		<add name="resolver://host/queue-2" uri="rabbitmq://user:password@the-server/inbox-work-queue" />
	</uriResolver>
```

Finally just close the relevant tags.

```xml
  </serviceBus>
</configuration>
```

You may wish to consider using the [TransactionScope](http://shuttle.github.io/shuttle-core/overview-transactionscope/) section to configure transactional behaviour for your endpoint.
