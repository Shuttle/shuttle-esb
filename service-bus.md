---
title: Shuttle-ESB API
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

## Sending **command** messages

Once you have an instance of the service bus available it is a matter of calling `Send` on the instance:

---
``` c#
TransportMessage Send(object message)
```
This method is the most typical invocation and simply takes the message and sends it to the destination as determined by the `IMessageRouteProvider`.

---
``` c#
TransportMessage Send(object message, string uri)
```
Using this method a message can be sent to the specified `Uri`.  *More commonly used by Shuttle-ESB itself*.

---
``` c#
TransportMessage Send(object message, IQueue queue)
```
Should you have a specific `IQueue` instance available you could send the message directly there. *More commonly used by Shuttle-ESB itself*.

---
``` c#
TransportMessage SendLocal(object message)
```
The given message will be sent back to the inboc work queue of this endpoint.

---
``` c#
TransportMessage SendReply(object message)
```
This method is only available when handling a message and will send the given message back to the `RecipientInboxWorkQueueUri` of the `TransportMessage` being handled.

## Sending deferred **command** messages

Sending deferred messages is functionally identical to sending message normally but the messages will only be processed once the given `DateTime` has been reached.  

When making use of deferred messages the endpoint that is responsible for processing the deferred message should ideally have `deferredQueueUri` configured for the inbox:

``` xml
	<serviceBus>
		<inbox
		  workQueueUri="msmq://./pubsub-client-inbox-work"
		  deferredQueueUri="msmq://./pubsub-client-inbox-deferred" <!-- THE DEFERRED QUEUE STORE -->
		  errorQueueUri="msmq://./shuttle-samples-error" />
	</serviceBus>
```

The processor on the deferred queue is optimized to process only when a message is due.  This leads to less IO than *not* having it.  If there is no deferred queue the message goes tot he work queue and will be ignored and may lead o sub-optimal processing of the work queue.

All you need to do now is call the relevant `SendDeferred` method on the available bus instance:

---
``` c#
TransportMessage SendDeferred(DateTime at, object message)
```
This method is the most typical invocation and simply takes the message and sends it to the destination as determined by the `IMessageRouteProvider`.

---
``` c#
TransportMessage SendDeferred(DateTime at, object message, string uri)
```
Using this method a message can be sent to the specified `Uri`.  *More commonly used by Shuttle-ESB itself*.

---
``` c#
TransportMessage SendDeferred(DateTime at, object message, IQueue queue)
```
Should you have a specific `IQueue` instance available you could send the message directly there. *More commonly used by Shuttle-ESB itself*.

---
``` c#
TransportMessage SendDeferredLocal(DateTime at, object message)
```
The given message will be sent back to the inboc work queue of this endpoint.

---
``` c#
TransportMessage SendDeferredReply(DateTime at, object message)
```
This method is only available when handling a message and will send the given message back to the `RecipientInboxWorkQueueUri` of the `TransportMessage` being handled.

## Publishing **event** messages

In order to notify other endpoints of an event that has occurred you need to `Publish` the event on the available bus isntance:

---
``` c#
IEnumerable<string> Publish(object message)
```
The service bus will make use of the registered `ISubscriptionManager` instance to obtain the `Uri`s of the subscribers that a copy of the message needs to be sent to.  The list of subscriber `Uri`s will be returned.

You will notice that there is no way to publish a deferred event.  This is because the event has already occurred and should a particular endpoint require deferred processing as a result then *that* endpoint can do a `SendDeferredLocal` to effect the required deferred processing.

# ServiceBusConfiguration

The `ServiceBusConfiguration` instance contains all the configuration required by the `ServiceBus` to operate.  In order to build the configuration you can make use of the `ServiceBusConfigurationBuilder` class.  To obtain an instance you can either `new` one up or call `ServiceBus.Create()`.  The `Start()` method of the `ServiceBusConfigurationBuilder` will create a new `ServiceBus` instance and call `Start()` on the newly instanced `ServiceBus`.

The simplest possible way to create and start a service bus is as follows:

``` c#
	bus = ServiceBus
		.Create()
		.Start();
```

All the default options will be used in such as case but there will be rather few occasions where this will suffice.  For instance, when you need to subscribe to an event or publish an event you will need an imeplementation of the `ISubscriptionManager`.  A typical call to create and the start a service bus using a Sql Server subscription manager is as follows:

``` c#
	var subscriptionManager = SubscriptionManager.Default();

	subscriptionManager.Subscribe(new[] { typeof(SomeInterestingEvent).FullName });

	bus = ServiceBus
		.Create()
		.SubscriptionManager(subscriptionManager)
		.Start();
```

### TransactionScopeFactory

[Go to the Transaction Scope Factory documentation]({{ site.baseurl }}/transactionscope-factory/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		IServiceBusTransactionScopeFactory TransactionScopeFactory { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder TransactionScopeFactory(IServiceBusTransactionScopeFactory serviceBusTransactionScopeFactory);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			TransactionScopeFactory = new DefaultServiceBusTransactionScopeFactory();
		}
	}
```

### PipelineFactory

[Go to the Pipeline Factory documentation]({{ site.baseurl }}/pipeline-factory/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		IPipelineFactory PipelineFactory { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder PipelineFactory(IPipelineFactory pipelineFactory);
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

[Go to the Message Route Provider documentation]({{ site.baseurl }}/message-route-provider/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		IMessageRouteProvider MessageRouteProvider { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder MessageRouteProvider(IMessageRouteProvider messageRouteProvider);
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

[Go to the Message Handler Factory documentation]({{ site.baseurl }}/message-handler-factory/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		IMessageHandlerFactory MessageHandlerFactory { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder MessageHandlerFactory(IMessageHandlerFactory messageHandlerFactory);
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

[Go to the Serializer documentation]({{ site.baseurl }}/serializer/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		ISerializer Serializer { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder MessageSerializer(ISerializer serializer);
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public ServiceBusConfiguration()
		{
			Serializer = new DefaultSerializer();
		}
	}
```

### ForwardingRouteProvider

[Go to the Message Route Provider documentation]({{ site.baseurl }}/message-route-provider/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		IMessageRouteProvider ForwardingRouteProvider { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder ForwardingRouteProvider(IMessageRouteProvider forwardingRouteProvider);
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

[Go to the Service Bus Policy documentation]({{ site.baseurl }}/service-bus-policy/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		IServiceBusPolicy Policy { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder Policy(IServiceBusPolicy policy);
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

[Go to the Thread Activity Factory documentation]({{ site.baseurl }}/thread-activity-factory/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		IThreadActivityFactory ThreadActivityFactory { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder ThreadActivityFactory(IThreadActivityFactory factory);
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

[Go to the Subscription Manager documentation]({{ site.baseurl }}/subscription-manager/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		ISubscriptionManager SubscriptionManager { get; }
		bool HasSubscriptionManager { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder SubscriptionManager(ISubscriptionManager manager);
	}
```

### Encryption

[Go to the Encryption documentation]({{ site.baseurl }}/encryption/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		void AddEncryptionAlgorithm(IEncryptionAlgorithm algorithm);
		IEncryptionAlgorithm FindEncryptionAlgorithm(string name);
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder AddEnryptionAlgorithm(IEncryptionAlgorithm algorithm);
	}
```

### Compression

[Go to the Compression documentation]({{ site.baseurl }}/compression/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		void AddCompressionAlgorithm(ICompressionAlgorithm algorithm);
		ICompressionAlgorithm FindCompressionAlgorithm(string name);
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder AddCompressionAlgorithm(ICompressionAlgorithm algorithm);
	}
```

### Modules

[Go to the Modules documentation]({{ site.baseurl }}/modules/index.html).

``` c#
	public interface IServiceBusConfiguration
	{
		ModuleCollection Modules { get; }
	}

	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder AddModule(IModule module);
	}
```