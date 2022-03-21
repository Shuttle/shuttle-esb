# Modules

Shuttle.Esb is extensible via modules. These plug into a relevant pipeline to perform additional tasks within the pipeline by registering one or more observers that respond to the events raised by the pipeline.

## Implementation

A module is an arbitrary class that should use the `IPipelineFactory` implementation to hook onto the relevant pipeline.

``` c#
public class LogMessageOwnerModule
{
    private readonly LogMessageOwnerObserver _logMessageOwnerObserver;
    private readonly string _inboxMessagePipelineName = typeof(InboxMessagePipeline).FullName;

    public void LogMessageOwnerModule(IPipelineFactory pipelineFactory, LogMessageOwnerObserver logMessageOwnerObserver)
    {
        Guard.AgainstNull(pipelineFactory, "pipelineFactory");
        Guard.AgainstNull(logMessageOwnerObserver, "logMessageOwnerObserver");
        
        _logMessageOwnerObserver = logMessageOwnerObserver;

        pipelineFactory.PipelineCreated += PipelineCreated;
    }

    private void PipelineCreated(object sender, PipelineEventArgs e)
    {
        if (!e.Pipeline.GetType().FullName.Equals(_inboxMessagePipelineName, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        e.Pipeline.RegisterObserver(_logMessageOwnerObserver);
    }
}
```

You may be wondering where the `LogMessageOwnerObserver` instance would come from.  The `Shuttle.Esb.ComponentRegistryExtensions.RegisterServiceBus()` method registers, in the `IComponentRegistry` instance, all observer types that it finds in the application.  However, to be sure that the relevant dependencies for you modules are registered you could use the `IComponentRegistry` instance to directly register them:

```c#
registry.AttemptRegister<LogMessageOwnerModule>();
registry.AttemptRegister<LogMessageOwnerObserver>();
```

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

## Pipelines

You can reference the [Shuttle.Esb](https://github.com/Shuttle/Shuttle.Esb/tree/master/Shuttle.Esb/Pipeline/Pipelines) code directly to get more information on the available pipelines and the events in those pipelines.

More information on the pipelines infrastructure can be obtained in the [Shuttle.Core.Pipelines](https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-pipelines.html) documentation.
