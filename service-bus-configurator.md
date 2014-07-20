---
title: Service Bus Configurator
layout: api
---
# ServiceBusConfigurator

A `ServiceBusConfigurator` is used to configure an instance of the [ServiceBusConfiguration].

## Methods

### Configuration

``` c#
public IServiceBusConfiguration Configuration()
```

Returns the configured [ServiceBusConfiguration] instance.

### MessageSerializer

``` c#
public ServiceBusConfigurator MessageSerializer(ISerializer serializer)
```

Specify the [ISerializer] instance that will be used for serialize and deserialize [TransportMessage] and the contained `Message` instances.

### MessageHandlerFactory

``` c#
public ServiceBusConfigurator MessageHandlerFactory(IMessageHandlerFactory messageHandlerFactory)
```

Specify the [IMessageHandlerFactory] that will be used to obtain [IMessageHandler] implementations.

### AddCompressionAlgorithm

``` c#
public ServiceBusConfigurator AddCompressionAlgorithm(ICompressionAlgorithm algorithm)
```

Registers a [compression] algorithm that may e used to either encrypt outgoing messages or decrypt incomming messages.

### AddEnryptionAlgorithm

``` c#
public ServiceBusConfigurator AddEnryptionAlgorithm(IEncryptionAlgorithm algorithm)
```

Registers an [encryption] alggorithm that may be used to either compress outgoing messages or to decompress incomming messages.

### SubscriptionManager

``` c#
public ServiceBusConfigurator SubscriptionManager(ISubscriptionManager manager)
```

Specify the [ISubscriptionManager] implementation to use for the endpoint.

### AddModule

``` c#
public ServiceBusConfigurator AddModule(IModule module)
```

Adds a [module] that can extend a pipeline and respond to pipeline events.

### Policy

``` c#
public ServiceBusConfigurator Policy(IServiceBusPolicy policy)
```

Specify the [IServiceBusPolicy] implementation that will determine retries.

### MessageRouteProvider

``` c#
public ServiceBusConfigurator MessageRouteProvider(IMessageRouteProvider messageRouteProvider)
```

The [IMessageRouteProvider] instance that is used to determine where to `Send` a message type to.

### ForwardingRouteProvider

``` c#
public ServiceBusConfigurator ForwardingRouteProvider(IMessageRouteProvider forwardingRouteProvider)
```

The [IMessageRouteProvider] instance that is used to determine where to forward a processed message to.

### ThreadActivityFactory

``` c#
public ServiceBusConfigurator ThreadActivityFactory(IThreadActivityFactory factory)
```

The [IThreadActivityFactory] instance that will return `ThreadActivity` instance.  *You will probably never specify this*. 

### PipelineFactory

``` c#
public ServiceBusConfigurator PipelineFactory(IPipelineFactory pipelineFactory)
```

Specify the [IPipelineFactory] implementation that will return instances of the relevant `MessagePipeline`.  *You will probably never specify this*.

### TransactionScopeFactory

``` c#
public ServiceBusConfigurator TransactionScopeFactory(ITransactionScopeFactory transactionScopeFactory)
```

The [ITransactionScopeFactory] instance is used to create `ITransactionScope` instances for placing code within a `TransactionScope`.  *You will probably never specify this*.

### IdempotenceService

``` c#
public ServiceBusConfigurator IdempotenceService(IIdempotenceService idempotenceService)
```

The [IdempotenceService] instance is used to ensure that *technically* identical messages (with the same message id) are not processed more than once.  It also ensures that messages sent within a transaction scope are only sent after the transaction scope has been commited.

[Compression]: {{ BASE_PATH }}/compression/index.html
[Encryption]: {{ BASE_PATH }}/encryption/index.html
[IMessageHandler]: {{ BASE_PATH }}/message-handler/index.html
[IMessageHandlerFactory]: {{ BASE_PATH }}/message-handler-factory/index.html
[IMessageRouteProvider]: {{ BASE_PATH }}/message-route-provider/index.html
[IPipelineFactory]: {{ BASE_PATH }}/pipeline-factory/index.html
[IThreadActivityFactory]: {{ BASE_PATH }}/thread-activity-factory/index.html
[ITransactionScopeFactory]: {{ BASE_PATH }}/transactionscope-factory/index.html
[IServiceBusPolicy]: {{ BASE_PATH }}/service-bus-policy/index.html
[ISubscriptionManager]: {{ BASE_PATH }}/subscription-manager/index.html
[Module]: {{ BASE_PATH }}/modules/index.html
[ServiceBusConfiguration]: {{ BASE_PATH }}/service-bus-configuration/index.html
[TransportMessage]: {{ BASE_PATH }}/transport-message/index.html
[IdempotenceService]: {{ BASE_PATH }}/idempotence-service/index.html