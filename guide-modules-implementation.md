---
title: Modules
layout: guide
---
# Overview

Shuttle.Esb is extensible via modules.  These typically plug into a relevant pipeline to perform additional tasks within the pipeline.

## Implementation

A module is an arbitrary class that should use the `IPipelineFactory` implementation to hook onto the relevant pipeline.  The easiest way to accomplish this is to use constructor injection along with [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping).

``` c#
public class LogMessageOwnerModule
{
	private readonly LogMessageOwnerObserver _logMessageOwnerObserver;
	private readonly string _inboxMessagePipelineType = typeof(InboxMessagePipeline);

	public LogMessageOwnerModule(IPipelineFactory pipelineFactory, LogMessageOwnerObserver logMessageOwnerObserver)
	{
		Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
		Guard.AgainstNull(logMessageOwnerObserver, nameof(logMessageOwnerObserver));
		
		_logMessageOwnerObserver = logMessageOwnerObserver;

		pipelineFactory.PipelineCreated += PipelineCreated;
	}

	private void PipelineCreated(object sender, PipelineEventArgs e)
	{
		if (!e.Pipeline.GetType().Equals(_inboxMessagePipelineType))
		{
			return;
		}

		e.Pipeline.RegisterObserver(_logMessageOwnerObserver);
	}
}
```

You may be wondering where the `LogMessageOwnerObserver` instance would come from.  The `ServiceBus.Regiser()` method registers all observer types that it finds in the application.

Here we have created a new module that registers the `LogMessageOwnerObserver` for each newly created `InboxMessagePipeline`.  Since a pipeline simply raises `PipelineEvent` instances the observer will need to listen out for the relevant events.  We will log the message owner after the transport message has been deserialized:

``` c#
public class LogMessageOwnerObserver : IPipelineObserver<OnAfterDeserializeTransportMessage>
{
	public void Execute(OnDeserializeTransportMessage pipelineEvent)
	{
		var state = pipelineEvent.Pipeline.State;
		var transportMessage = state.GetTransportMessage();
		
		if (transportMessage == null)
		{
			return;
		}
		
		Console.Log("This transport message belongs to '{0}'.", transportMessage.PrincipalIdentityName);
	}
}
```

Each pipeline has a state that contains various items.  You can add state and there are some extensions on the state that return various well-known items such as `GetTransportMessage()` that returns the `TransportMessage` on the pipeline.  Prior to deserializing the transport message it will, of course, be `null`.

Pipelines are re-used so they are created as needed and returned to a pool.  Should a pipeline be retrieved from the pool it will be re-initialized so that the previous state is removed.

## Using our module

To make using your module easy for anyone needing it you can make use of the [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping) provided by the `Shuttle.Core.Infrastructure` package.  This will automatically register the module and resolve it in order to wire up everything correctly:

``` c#
public class Bootstrap :
	IComponentRegistryBootstrap,
	IComponentResolverBootstrap
{
	private static bool _registryBootstrapCalled;
	private static bool _resolverBootstrapCalled;

	public void Register(IComponentRegistry registry)
	{
		Guard.AgainstNull(registry, "registry");

		if (_registryBootstrapCalled)
		{
			return;
		}

		registry.AttemptRegister<LogMessageOwnerModule>();
		registry.AttemptRegister<LogMessageOwnerObserver>();

		_registryBootstrapCalled = true;
	}

	public void Resolve(IComponentResolver resolver)
	{
		Guard.AgainstNull(resolver, "resolver");

		if (_resolverBootstrapCalled)
		{
			return;
		}

		resolver.Resolve<LogMessageOwnerModule>();

		_resolverBootstrapCalled = true;
	}
}
```

# Pipelines

You can refernce the [Shuttle.Esb](https://github.com/Shuttle/Shuttle.Esb/tree/master/Shuttle.Esb/Pipeline/Pipelines) code directly to get more information on the available pipelines and the events in those pipelines.
