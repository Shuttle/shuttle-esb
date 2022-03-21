# Message Forwarding

```
PM> Install-Package Shuttle.Esb.Module.MessageForwarding
```

The MessageForwarding module for Shuttle.Esb will forward any handled messages onto the specified queue(s).

The module will attach the `MessageForwardingObserver` to the `OnAfterHandleMessage` and then send the handled message on to any defined endpoints.

```xml
<configuration>
	<configSections>
		<section name="messageForwarding" type="Shuttle.Esb.Module.MessageForwarding.MessageForwardingSection, Shuttle.Esb.Module.MessageForwarding"/>
	</configSections>

	<messageForwarding>
		<forwardingRoutes>
			<messageRoute uri="msmq://./inbox">
				<add specification="StartsWith" value="Shuttle.Messages1" />
				<add specification="StartsWith" value="Shuttle.Messages2" />
			</messageRoute>
			<messageRoute uri="sql://./inbox">
				<add specification="TypeList" value="DoSomethingCommand" />
			</messageRoute>
		</forwardingRoutes>
	</messageForwarding>
</configuration>
```

## Registration / Activation

The required components may be registered by calling `ComponentRegistryExtensions.RegisterMessageForwarding(IComponentRegistry)`.

In order for the module to attach to the `IPipelineFactory` you would need to resolve it using `IComponentResolver.Resolve<MessageForwardingModule>()`.
