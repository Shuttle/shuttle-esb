# Message Route Provider

An implementation of the `IMessageRouteProvider` interface is used to obtain a list of the uris that a message should be routed to.

The `DefaultMessageRouteProvider` makes use if the `ServiceBusOptions` to determine the routing.  You could implement this interface for custom routing such as a centrally managed database containing the routing.

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

The `MessageRouteProvider` obtains its configuration from the [MessageRouteOptions](/shuttle-esb/options/message-routes.html) defined on the `ServiceBusOptions`.

### Specifications

For each message route you need to specify the specification to apply to the value.  The [specification](http://en.wikipedia.org/wiki/Specification_pattern) will determine wether the route will be added.

#### StartsWithMessageRouteSpecification

The `StartsWithMessageRouteSpecification` will include the route when the message type's full name starts with the given `value`

#### TypeListMessageRouteSpecification

For the `TypeListMessageRouteSpecification` you need to provide a list of the assembly-qualified name of the type or types (semi-colon delimited).

#### RegexMessageRouteSpecification

For the `RegexMessageRouteSpecification` you need to provide the [regex](http://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex.aspx) expression to apply to the full name of the type.

#### AssemblyMessageRouteSpecification

The `AssemblyMessageRouteSpecification` is derived from the `TypeListMessageRouteSpecification` and includes all the types from the assembly represented by `value`.  If the assembly provided by `value` ends with either `.exe` or `.dll` the assembly will be loaded from the given path, else the assembly will be loaded from the `AppDomain` (so it should be referenced).
