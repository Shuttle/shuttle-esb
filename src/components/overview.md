# Components

Shuttle.Esb is highly configurable and all the major components have been abstracted behind interfaces. As such it is possible to replace any of the components withh custom implementations if required.

In order to replace a component you would need to register it before invoking the `Shuttle.Esb.ComponentRegistryExtensions.RegisterServiceBus()` method since the `RegisterServiceBus()` method makes use of the `IComponentRegistry.AttemptRegister()` method which would ignore a registration if it already exists.

You can also register message handlers separately from any `Assembly` using:

```c#
public static void RegisterMessageHandlers(this IComponentRegistry registry, Assembly assembly);
```