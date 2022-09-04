# Upgrade to v-13.0.0

## Shuttle.Core.Container

Prior to version `13.0.0` all `Shuttle.*` components supported dependency injection via an adapter based on [Shuttle.Core.Container](https://github.com/Shuttle/Shuttle.Core.Container) and would typically register dependencies using a `IComponentRegistry.Register*` method.  

You would have to choose and implementation, such as `Shuttle.Core.Ninject`, in order to support dependency injection.

From v-13.0.0 of `Shuttle.Esb` only [.NET dependency injection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection) is supported and the configuration of components has been refactored to make use of the [options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0).

## Options

As per the .Net dependency injection guidelines the components are added using an `IServiceCollection.Add{Library}` method.  When there are options applicable to the component a `Builder` may be used to facilitate configuration, e.g.:

```c#
var services = new ServiceCollection();

service.AddLibrary(builder =>
{
    builder.ConfigurationMethod(args);

    builder.Options.option = optionValue;
})
```

Please see the documentation related to each component for specifics.

The `ServiceBus` instance no longer needs to be managed as the instance lifecycle is managed by the `ServiceBusHostedService`.

## Queue configuration

All queues URIs are now structured as `scheme://configuration-name/queue-name` and the `scheme` represents a unique name for the `IQueue` implementation.  The `scheme` and `configuration-name` (represented by the URI's `Host` property) should always be lowercase as creating a `new Uri(uriString)` forces the scheme and host to lowercaseThe intention is to allow easier configuration of queue properties.

This does mean, however, that all existing queue configuration would need to be refactored, including subscriptions.

## Shuttle.Esb.AzureMQ

This package has been renamed to `Shuttle.Esb.AzureStorageQueues` and the scheme has been changed to `azuresq` from `azuremq`.

## Streams

The `IQueue` interface now has an `IsStream` boolean property that indicates whether or not the queue represents a stream such as `Shuttle.Esb.Kafka`.

## Discord

Should you have any questions or comments you are welcome to visit the [Discord channel](https://discord.gg/Q2yEsfht6f).