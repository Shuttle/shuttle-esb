---
title: Corrupt Transport Message Module
layout: api
---
# Corrupt Transport Message Module

```
PM> Install-Package Shuttle.Esb.Module.CorruptTransportMessage
```

The CorruptTransportMessage module for Shuttle.Esb writes any transport messages that fail to deserialize to disk.

It will log any transport messages that fail deserailization via the `ServiceBusEvents.TransportMessageDeserializationException` event to a folder as specified in the configuration:

```xml
<configuration>
    <configSections>
        <section name="corruptTransportMessage" type="Shuttle.Esb.Module.CorruptTransportMessage.CorruptTransportMessageSection, Shuttle.Esb.Module.CorruptTransportMessage"/>
    </configSections>

  <corruptTransportMessage folder=".\corrupt-transport-messages" />
</configuration>
```

The default value for the `folder` attribute is `.\corrupt-transport-messages`.

The module will register itself using the [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping).
