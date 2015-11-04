---
title: Message Forwarding Module
layout: api
---
# Message Forwarding Module

The `MessageForwardingModule` may be found in the `Shuttle.ESB.Modules` assembly.  The module will attach the `MessageForwardingObserver` to the `OnAfterHandleMessage` and then send the handled message on to any defined endpoints.

```xml
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
```

```c#
	var bus = ServiceBus
		.Create()
		.AddModule(new MessageForwardingModule())
		.Start();
```

The specifications are the same specifications used by the `DefaultMessageRouteProvider` implementation.