---
title: Serializer
layout: api
---
# ISerializer

An implementation of the `ISerializer` interface is used to serialize objects, typically messages, into a `Stream`.

The `DefaultSerializer` makes use of the standard .NET xml serialization functionality.

## Methods

### Serialize

``` c#
Stream Serialize(object message);
```

Returns the message `object` as a `Stream`.

### Deserialize

``` c#
object Deserialize(Type type, Stream stream);
```

Deserializes the `Stream` into an `obejct` of the given type.

