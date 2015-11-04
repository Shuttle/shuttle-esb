---
title: Message Route Provider
layout: api
---
# IMessageRouteProvider

An implementation of the `IMessageRouteProvider` interface is used to obtain a list of the uris that a message should be routed to.

There are two default implementations of the interface that are used for different purposes.  Should you ever require a custom mechanism to return the uris you simply provide the configuration with the relevant instance.  An example may be a centrally managed database containing the routing.

## Methods

### GetRouteUris

``` c#
IEnumerable<string> GetRouteUris(object message);
```

The method will return the list of uris that the message should be routed to.  All qualifying routes should be added to the resulting collection of uris.

### Add

``` c#
void Add(IMessageRoute messageRoute);
```

The method will add a new `IMessageRoute` instance.

## Implementation

### DefaultMessageRouteProvider

The `DefaultMessageRouteProvider` obtains its configuration from the application configuration file:

```xml
    <messageRoutes>
      <messageRoute uri="msmq://./inbox">
        <add specification="StartsWith" value="Shuttle.Messages1" />
        <add specification="StartsWith" value="Shuttle.Messages2" />
      </messageRoute>
      <messageRoute uri="sql://./inbox">
        <add specification="TypeList" value="DoSomethingCommand" />
      </messageRoute>
    </messageRoutes>
```

### DefaultForwardingRouteProvider

The `DefaultForwardingRouteProvider` also obtains its configuration from the application configuration file:

```xml
    <forwardingRoutes>
      <messageRoute uri="msmq://./inbox">
        <add specification="StartsWith" value="Shuttle.Messages1" />
        <add specification="StartsWith" value="Shuttle.Messages2" />
      </messageRoute>
      <messageRoute uri="sql://./inbox">
        <add specification="TypeList" value="DoSomethingCommand" />
      </messageRoute>
    </forwardingRoutes>
```

### Specifications

For each message route you need to specify the specification to apply to the value.  The [specification](http://en.wikipedia.org/wiki/Specification_pattern) will determine wether the route will be added.

#### StartsWithMessageRouteSpecification

The `StartsWithMessageRouteSpecification` will include the route when the message type's full name starts with the given `value`:

``` xml
    <messageRoutes>
      <messageRoute uri="msmq://./inbox">
        <add specification="StartsWith" value="Shuttle.Messages1" />
      </messageRoute>
    </messageRoutes>
```
---
#### TypeListMessageRouteSpecification

For the `TypeListMessageRouteSpecification` you need to provide a list of the assembly-qualified name of the type or types (semi-colon delimited).

```xml
    <messageRoutes>
      <messageRoute uri="rabbitmq://shuttle:shuttle!@localhost/inbox">
        <add specification="TypeList" value="Shuttle.Messages1.SomeMessage, Shuttle.Message1;Shuttle.Messages2.SomeMessage, Shuttle.Message2" />
      </messageRoute>
    </messageRoutes>
```
---
#### RegexMessageRouteSpecification

For the `RegexMessageRouteSpecification` you need to provide the [regex](http://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex.aspx) expression to apply to the full name of the type.

```xml
    <messageRoutes>
      <messageRoute uri="rabbitmq://shuttle:shuttle!@serverA/inbox">
        <add specification="Regex" value="^Shuttle\.Messages[12]" />
      </messageRoute>
    </messageRoutes>
```
---
#### AssemblyMessageRouteSpecification

The `AssemblyMessageRouteSpecification` is derived from the `TypeListMessageRouteSpecification` and includes all the types from the assembly represented by `value`.  If the assembly provided by `value` ends with either `.exe` or `.dll` the assembly will be loaded from the given path, else the assembly will be loaded from the `AppDomain` (so it should be referenced).

``` xml
    <messageRoutes>
      <messageRoute uri="msmq://./inbox">
        <add specification="assembly" value="Shuttle.Messages1" />
      </messageRoute>
      <messageRoute uri="rabbitmq://shuttle:shuttle!@serverA/inbox">
        <add specification="assembly" value="Shuttle.Messages2.dll" />
      </messageRoute>
    </messageRoutes>
```
