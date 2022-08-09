# Message Routing

Once you have instantiated a message you need to get it to a specific endpoint.  You can let Shuttle.Esb decide this for you implicitly by configuring a routing mechanism or you can even specify the endpoint explicitly.

Typically when sending a message that message is a command.  It does not _have_ to be a command and you _can_ send an event message to a specific endpoint but more-often-than-not you will be sending a command.  Messages are sent by calling one of the relevant overloaded methods on the service bus instance:

```c#
TransportMessage Send(object message, Action<TransportMessageBuilder> builder = null);
```

Only messages that have no `RecipientInboxWorkQueueUri` set will be routed by the service bus.

The `TransportMessage` envelope will be returned should you need access to any of the metadata available for the message.

Shuttle.Esb uses an implementation of an `IMessageRouteProvider` to determine where messages are sent.

```c#
public interface IMessageRouteProvider
{
    IEnumerable<string> GetRouteUris(object message);    
}
```

## Implementation

The `DefaultMessageRouteProvider` is registered if no `IMessageRouteProvider` has been registered and makes use of the [message routing options](/options/message-routes) to determine where to send messages:

Each implementation of `IMessageRouteProvider` can determine the routes however it needs to, from the given message.  A typical scenario, and the way the `DefaultMessageRouteProvider` works, is to use the full type name to determine the destination.

**Please note**: each message type may only be sent to _one_ endpoint using `Send`.
