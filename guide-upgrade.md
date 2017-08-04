---
title: Upgrade Guide
layout: guide
---
# Overview

From ***v8.0.0*** of Shuttle.Esb there is a strong focus around a dependency injection container.  Although the core functionality of this version is the same as previous versions there has been a significant change in how the components are wired up.  In most instances this may ease some of the configuration.

## Selecting a container

To start off with you'll need to pick one of the [supported containers](http://shuttle.github.io/shuttle-core/overview-container/#Supported):

<div class="nuget-badge">
	<p>
		<code>Install-Package Shuttle.Core.Autofac</code>
	</p>
</div>

## Shuttle.Esb.SqlServer is obsolete

This package has been split into the following distnct packages:

- Shuttle.Esb.Sql.Subscription
- Shuttle.Esb.Sql.Idempotence
- Shuttle.Esb.Sql.Queue

From this you probably gather that each only focuses on the specifically required functionality.  As such, you will need to reference the one that you are after and remove the `Shuttle.Esb.SqlServer` reference.

The idea behind the `Shuttle.Esb.Sql` packages is that they will contain a implementation for various sql-based data stores.  For now it is only Sql Server but in time it will contain others also.

## Refactoring the configuration

### Simplest form

Change the pre-v8.0 version:

``` c#
public class Host : IHost, IDisposable
{
	private IServiceBus _bus;

	public void Start()
	{
		_bus = ServiceBus.Create().Start();
	}

	public void Dispose()
	{
		_bus.Dispose();
	}
}
```

To the container focused version (using `WindsorContainer` here):

``` c#
public class Host : IHost, IDisposable
{
	private IServiceBus _bus;

	public void Start()
	{
		var container = new WindsorComponentContainer(new WindsorContainer());

		// ServiceBus.Register(IComponentRegistry registry)
		ServiceBus.Register(container);

		// ServiceBus.Create(IComponentResolver resolver)
		_bus = ServiceBus.Create(container).Start();
	}

	public void Dispose()
	{
		_bus.Dispose();
	}
}
```

### Message handler factories

These are no longer required as the handlers are registered in the container proper.  You can safely just remove the configuration.  This is an example of where there is less configuration to perform directly on the service bus creation.

Change the pre-v8.0 version:

``` c#
public class Host : IHost, IDisposable
{
	private IServiceBus _bus;
	private WindsorContainer _container;

	public void Start()
	{
		_container = new WindsorContainer();

		_container.Register(Component.For<IEMailService>().ImplementedBy<EMailService>());

		_bus = ServiceBus.Create(
			c => c.MessageHandlerFactory(new CastleMessageHandlerFactory(_container))
			).Start();
	}

	public void Dispose()
	{
		_bus.Dispose();
		_container.Dispose();
	}
}
```

To the container focused version (using `Ninject` here):

``` c#
public class Host : IHost, IDisposable
{
	private IServiceBus _bus;
	private StandardKernel _kernel;

	public void Dispose()
	{
		_kernel.Dispose();
		_bus.Dispose();
	}

	public void Start()
	{
		_kernel = new StandardKernel();

		var container = new NinjectComponentContainer(_kernel);

		// ServiceBus.Register(IComponentRegistry registry)
		ServiceBus.Register(container);

		// ServiceBus.Create(IComponentResolver resolver)
		_bus = ServiceBus.Create(container).Start();
	}
}
```

### Subscription manager

This is going to depend on the implementation but if all implementors follow the convention then you no longer need to specifically register the subscription manager.  In the case of using `Shuttle.Esb.Sql.Subscription` (see the section about `Shuttle.Esb.SqlServer` being obsolete) it'll be as follows:

Change the pre-v8.0 version:

``` c#
public class Host : IHost, IDisposable
{
	private IServiceBus _bus;

	public void Start()
	{
		var subscriptionManager = SubscriptionManager.Default();
	
		subscriptionManager.Subscribe<SomeEvent>();
	
		_bus = ServiceBus.Create(c => c.SubscriptionManager(subscriptionManager)).Start();
	}

	public void Dispose()
	{
		_bus.Dispose();
	}
}
```

To the container focused version (using `WindsorContainer` here):

``` c#
public class Host : IHost, IDisposable
{
	private IServiceBus _bus;

	public void Start()
	{
		var structureMapRegistry = new Registry();
		var registry = new StructureMapComponentRegistry(structureMapRegistry);

		// ServiceBus.Register(IComponentRegistry registry)
		ServiceBus.Register(registry);

		var resolver = new StructureMapComponentResolver(new Container(structureMapRegistry));

		resolver.Resolve<ISubscriptionManager>().Subscribe<SomeEvent>();

		// ServiceBus.Create(IComponentResolver resolver)
		_bus = ServiceBus.Create(resolver).Start();
	}

	public void Dispose()
	{
		_bus.Dispose();
	}
}
```

### Other registrations and ordering

There appears to be a move in the dependency injection container world where the registration of components has been split from the resolving of the dependencies.  Some still have both together and allow new registrations after even resolving a dependency.  Others have both together but don't allow new registrations after resolving a dependency.

All the reasoning behind these ideas does seem sound.

To this end the component interfaces have been moved to the `Shuttle.Core.Infrastructure` assembly and, taking the lowest common denominator, the registry has been split from resolver.

It is important to note that the ServiceBus registers quite a few components in the call to `ServiceBus.Register(IComponentRegistry registry)` including some null pattern dependencies.  An example of this would be the `ISubscriptionManager` implementation.  Should no implementation be registered for `ISubscriptionManager` the implementation is registered as `NullSubscriptionManager`.

### Configuration

For the most part the configuration has stayed the same apart from the `transactionScope` tag that has been removed from the `serviceBus` section and moved to the `Shuttle.Core.Infrastructure` package.  The [TransactionScope documentation](http://shuttle.github.io/shuttle-core/overview-transactionscope/) has more information.

### Bootstrapping

When calling `ServiceBus.Register(IComponentRegistry registry)` the registry [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping) is invoked *before* any components are registered by the `ServiceBus`.  This allows any other modules and boostrapped components to register themselves and *override* any subsequent registrations.

Consequently, calling the `ServiceBus.Create(IComponentResolver resolver)` method will invoke the resolver [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping).  This is particularly useful for components that need to be wired up (resolved) but that aren't necessarily a depen of any other component.  This is the case with modules that require the `IPipelineFactory` but they are not required as a dependency of any other component.