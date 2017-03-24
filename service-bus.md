---
title: Shuttle.Esb API
layout: api
---
# ServiceBus

There are basically three ways to communicate from one endpoint to another:

- sending **command** messages for processing
- sending **command** messages for *deferred* processing
- publishing **event** messages

A **command** message is only ever sent to **one** endpoint and at least **one** endpoint is required for the message to be successfully sent.  This requirement stems from a **command** being an instruction that will result in an action so it is something that is still going to take place.  If there is no endpoint to send a **command** to the instruction will not be executed whilst there is an expectation that the action would be executed.

An **event**, on the other hand, is something that has happened typically as a result of a **command** but not always.  There may be some other state that is noticed by the system that may require an **event** being published.  An event may have **zero** or **more** subscribers since there is no requirement for anyone to be interested in the event.  Typically when a business **event** is defined there should be at least one interested subscriber; else the **event** would not exist.  There may be some infrastructure-related eventst that may not always have a subscriber but it may be worthwhile publising the event anyway since it is something interesting that has occurred.

Some examples of *commands* and *events*:

| Command 					| Event 					|
| ---						| ---						|
| `SendEMailCommand`		| `EMailSentEvent`			|
| `CreateCustomerCommand`	| `CustomerCreatedEvent`	|
| `PlaceOrderCommand`		| `OrderPlacedEvent`		|
| `CancelOrderCommand`		| `OrderCancelledEvent`		|

## Message structure

Both **command** messages and **event** messages are basically data containers.  They are, therefore, data transfer objects and should not contain any behaviour.  If you would like to add some basic behaviour you should opt for extension methods as these will not interfere with serialization.  LINQ features, for example, may interfere with serialization.

Since the messages are plain classes you do not need to implement any specific interface or inherit from any specific class to make them work.

You may want to apply some convention to distinguish between the messages.  As indicated by the examples above a **Command** and **Event** suffix may be added to the **command** and **event** messages respectively.

In order to **send** a command or **publish** an event you need an instance of the [IMessageSender][MessageSender] interface.  This interface is implemented on both the `ServiceBus` class (via the `IServiceBus` interface) and the [HandlerContext].

# ServiceBusConfiguration

The `ServiceBusConfiguration` instance contains all the configuration required by the `ServiceBus` to operate.  In order to build the configuration you can make use of the [ServiceBusConfigurator] that is exposed on the `Create` method.  The `Create` method returns an instance of the `ServiceBus` and you can then call the `Start` method at the appropriate time.

The simplest possible way to create and start a service bus is as follows:

``` c#
	bus = ServiceBus
		.Create()
		.Start();
```

All the default options will be used in such as case but there will be rather few occasions where this will suffice.  For instance, when you need to subscribe to an event or publish an event you will need an imeplementation of the [ISubscriptionManager].  A typical call to create and the start a service bus using a Sql Server subscription manager is as follows:

``` c#
	var subscriptionManager = SubscriptionManager.Default();

	subscriptionManager.Subscribe(new[] { typeof(SomeInterestingEvent).FullName });

	bus = ServiceBus
		.Create(c => c.SubscriptionManager(subscriptionManager))
		.Start();
```

### TransactionScopeFactory

Go to the [ITransactionScopeFactory] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		ITransactionScopeFactory TransactionScopeFactory { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator TransactionScopeFactory(ITransactionScopeFactory TransactionScopeFactory);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			TransactionScopeFactory = new DefaultTransactionScopeFactory();
		}
	}
```

### PipelineFactory

Go to the [IPipelineFactory] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		IPipelineFactory PipelineFactory { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator PipelineFactory(IPipelineFactory pipelineFactory);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			PipelineFactory = new DefaultPipelineFactory();
		}
	}
```

### MessageRouteProvider

Go to the [IMessageRouteProvider] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		IMessageRouteProvider MessageRouteProvider { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator MessageRouteProvider(IMessageRouteProvider messageRouteProvider);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			MessageRouteProvider = new DefaultMessageRouteProvider();
		}
	}
```

### MessageHandlerFactory

Go to the [IMessageHandlerFactory] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		IMessageHandlerFactory MessageHandlerFactory { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator MessageHandlerFactory(IMessageHandlerFactory messageHandlerFactory);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			MessageHandlerFactory = new DefaultMessageHandlerFactory();
		}
	}
```

### Serializer

Go to the [ISerializer] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		ISerializer Serializer { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator MessageSerializer(ISerializer serializer);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			Serializer = new DefaultSerializer();
		}
	}
```
.

### ForwardingRouteProvider

Go to the [IMessageRouteProvider] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		IMessageRouteProvider ForwardingRouteProvider { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator ForwardingRouteProvider(IMessageRouteProvider forwardingRouteProvider);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			ForwardingRouteProvider = new DefaultForwardingRouteProvider();
		}
	}
```

### Policy

Go to the [IServiceBusPolicy] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		IServiceBusPolicy Policy { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator Policy(IServiceBusPolicy policy);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			Policy = new DefaultServiceBusPolicy();
		}
	}
```

### ThreadActivityFactory

Go to the [IThreadActivityFactory] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		IThreadActivityFactory ThreadActivityFactory { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator ThreadActivityFactory(IThreadActivityFactory factory);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			ThreadActivityFactory = new DefaultThreadActivityFactory();
		}
	}
```

### SubscriptionManager

Go to the [ISubscriptionManager] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		ISubscriptionManager SubscriptionManager { get; }
		bool HasSubscriptionManager { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator SubscriptionManager(ISubscriptionManager manager);
	}
```

### Encryption

Go to the [Encryption] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		void AddEncryptionAlgorithm(IEncryptionAlgorithm algorithm);
		IEncryptionAlgorithm FindEncryptionAlgorithm(string name);
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator AddEnryptionAlgorithm(IEncryptionAlgorithm algorithm);
	}
```

### Compression

Go to the [Compression] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		void AddCompressionAlgorithm(ICompressionAlgorithm algorithm);
		ICompressionAlgorithm FindCompressionAlgorithm(string name);
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator AddCompressionAlgorithm(ICompressionAlgorithm algorithm);
	}
```

### Modules

Go to the [Modules] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		ModuleCollection Modules { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator AddModule(IModule module);
	}
```
.
### IdempotenceService

Go to the [IdempotenceService] documentation.

``` c#
	public interface IServiceBusConfiguration
	{
		IIdempotenceService IdempotenceService { get; }
	}

	public class ServiceBusConfigurator
	{
        ServiceBusConfigurator IdempotenceService(IIdempotenceService idempotenceService);
	}
```

[Encryption]: {{ site.baseurl }}/encryption
[Compression]: {{ site.baseurl }}/compression
[HandlerContext]: {{ site.baseurl }}/handler-context
[IMessageHandlerFactory]: {{ site.baseurl }}/message-handler-factory
[IMessageRouteProvider]: {{ site.baseurl }}/message-route-provider
[IPipelineFactory]: {{ site.baseurl }}/pipeline-factory
[ISerializer]: {{ site.baseurl }}/serializer
[IServiceBusPolicy]: {{ site.baseurl }}/service-bus-policy
[ISubscriptionManager]: {{ site.baseurl }}/subscription-manager
[IThreadActivityFactory]: {{ site.baseurl }}/thread-activity-factory
[ITransactionScopeFactory]: {{ site.baseurl }}/transactionscope-factory
[MessageSender]: {{ site.baseurl }}/message-sender
[Modules]: {{ site.baseurl }}/modules
[ServiceBusConfigurator]: {{ site.baseurl }}/service-bus-configurator
[TransportMessage]: {{ site.baseurl }}/transport-message
[TransportMessageConfigurator]: {{ site.baseurl }}/transport-message-configurator
[IdempotenceService]: {{ site.baseurl }}/idempotence-service
