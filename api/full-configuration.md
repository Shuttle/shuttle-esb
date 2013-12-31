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

Attribute `removeMessagesNotHandled` indicates whether messages received on the endpoint that have no message handler should simply be removed (ignored).  If this attribute is `false` then the message will immmediately be placed in the error queue.

The `compressionAlgorithm` and `encryptionAlgorithm` attributes indicate the default compression and encryption algorithms to use when sending messages.

```xml
  <serviceBus
     removeMessagesNotHandled="true"
     compressionAlgorithm="GZip"
     encryptionAlgorithm="3DES">
```

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
      workQueueUri="msmq://./inbox_work"
      errorQueueUri="msmq://./inbox_error"
      journalQueueUri="msmq://./inbox_journal"
      workQueueStartupAction="Purge"
      threadCount="25"
      durationToSleepWhenIdle="250ms,10s,30s"
      durationToIgnoreOnFailure="30m,1h"
      maximumFailureCount="25" 
      distribute="true|false" />
```

For **real** queueing technologies the `outbox` should typically not be required.  It should be used in scenarios where you need a store-and-forward mechanism for sending messages when the underlying infrastructure does not provide this such as with a SqlServer table-based queue or maybe even the file system.

```xml
    <outbox
      workQueueUri="msmq://./outbox_work"
      errorQueueUri="msmq://./outbox_error"
      durationToSleepWhenIdle="250ms,10s,30s"
      durationToIgnoreOnFailure="30m,1h"
      maximumFailureCount="25"
      threadCount="5" />
```

When a [DeferredMessageQueue]({{ site.baseurl }}/api/deferred-messages.index) implementation has been specified you may want to specify the `deferredMessage` tag.

```xml
    <deferredMessage
      durationToSleepWhenIdle="250ms,1s,3s" />
```

You can also set the transaction scope behaviour by providing the `transactionScope` tag.  If you want your endpoint to be non-transactional then set the `enabled` attribute to `false`.  This will improve performance but messsage delivery and processing cannot be guaranteed.  If the underlying queueing infrastrcuture does not support 2-phase commit message delivery and processing also cannot be guaranteed.

```xml
    <transactionScope
      enabled="true"
      isolationLevel="ReadCommitted"
      timeoutSeconds="30" />
```

When the endpoint is not a physical endpoint but rather a worker use the `worker` tag to specify the relevant configuration.

```xml
    <worker
      distributorControlWorkQueueUri="msmq://./distributor-server-control-inbox-work"
      threadAvailableNotificationIntervalSeconds="5" />
  </serviceBus>
</configuration>
```
