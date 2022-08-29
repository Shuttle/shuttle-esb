# Components

Shuttle.Esb is highly configurable and all the major components have been abstracted behind interfaces. As such it is possible to replace any of the components withh custom implementations if required.

In order to replace a component you would need to register it before invoking the `services.AddServiceBus()` method since the relevant components are registered using the `service.Try*()` methods which would ignore a registration if it already exists.  There are some components that are not optionally registered and can therefore not be replaced.  If you find that you need to do so please log an issue in order to investigate the use-case.
