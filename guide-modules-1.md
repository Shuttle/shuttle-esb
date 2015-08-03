---
title: Modules
layout: guide
---
# Concepts

Shuttle-ESB is extensible via modules.  These typically plug into a relevant pipeline to perform additional tasks within the pipeline.

## Implementation

A module is an implementation of the `IModule` interface and this, in turn, implements the `IRequireInitialization` interface.

``` c#
    public class LogMessageOwnerModule : IModule
    {
		private readonly string _receiveMessagePipelineName = typeof(InboxMessagePipeline).FullName;

		public void Initialize(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			bus.Events.PipelineCreated += PipelineCreated;
		}

		private void PipelineCreated(object sender, PipelineEventArgs e)
		{
			if (!e.Pipeline.GetType().FullName.Equals(_inboxMessagePipelineName, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			e.Pipeline.RegisterObserver(new LogMessageOwnerObserver());
		}
    }
```

So here we have create a new module that registers the `LogMessageOwnerObserver` for each newly created `InboxMessagePipeline`.  Since a pipeline simply rasies `PipelineEvent` instances the observer will need to listen out for the relevant events.  We will log the message owner after the transport message has been deserialized:

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

Each pipeline has a state that contain various items.  You can add state and there are some extensions on the state that return various well-known items such as `GetTransportMessage()` that returns the `TransportMessage` on the pipeline.  Prior to deserializing the transport message it will, of course, be `null`.

Pipelines are re-used so they are created as needed and returned to a pool.  Should a pipeline be retrieved from the pool it will be re-initialized so that the previous state is removed.

To use the module it will need to be referenced in the relevant endpoint and added to the `Modules` collection for the service bus configuration

Using code:

``` c#
	bus = ServiceBus
		.Create(c => c.AddModule(new LogMessageOwnerModule()))
		.Start();
```

Via the configuration file:

``` xml
	<modules>
		<add type="Shuttle.ESB.Modules.LogMessageOwnerModule, Shuttle.ESB.Modules" />
	</modules>
```