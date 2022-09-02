# Upgrade to v-13.0.0 #

# Register 

Prior to version `12.0.0` the service bus components were registered using the static `Register()` method on the `ServiceBus` class:

```c#
ServiceBus.Register(IComponentRegistry);
```

The method is now marked as `Obsolete` but will still work until it is removed in the next version:

```c#
[Obsolete("This method has been replaced by `ComponentRegistryExtensions.RegisterServiceBus()`.", false)]
public static IServiceBusConfiguration Register(IComponentRegistry registry)
{
    return registry.RegisterServiceBus();
}
```

As can be seen in the above implementation the new way to register the service bus components with the component registry is to use the `RegisterServiceBus()` extension method on the `IComponentRegistry` implementation which has the following signature:

```c#
public static IServiceBusConfiguration RegisterServiceBus(this IComponentRegistry registry, IServiceBusConfiguration configuration = null)
```

# Resolve 

Prior to version 12.0.0 thhe service bus instance was obtained using the static `Create()` method on the ServiceBus class:

```c#
var serviceBus = ServiceBus.Create(IComponentResolver);
```

The method is now marked as `Obsolete` but will still work until it is removed in the next version:

```c#
[Obsolete("Please create an instance of the service bus using `IComponentResolver.Resolve<IServiceBus>()`.")]
public static IServiceBus Create(IComponentResolver resolver)
{
    Guard.AgainstNull(resolver, nameof(resolver));

    return resolver.Resolve<IServiceBus>();
}
```

As can be seen in the above implementation the new way to resolve and instance of the service bus is to use the `IComponentResolver` instance directly.