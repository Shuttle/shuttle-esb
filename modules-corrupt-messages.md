---
title: Corrupt Transport Message Module
layout: api
---
# Corrupt Transport Message Module

The `CorruptTransportMessageModule` may be found in the `Shuttle.ESB.Modules` assembly.  It will log any transport messages that fail deserailization via the `ServiceBus..Events.TransportMessageDeserializationException` event to a folder as specified in the application configuration `appSettings` key with name `CorruptTransportMessageFolder`:

~~~xml
  <appSettings>
    <add key="CorruptTransportMessageFolder" value="d:\shuttle-corrupt-messages"/>
  </appSettings>
~~~

~~~c#
	var bus = ServiceBus
		.Create.Create
		(
			c => c.AddModule(new CorruptTransportMessageModule())
		)
		.Start();
~~~