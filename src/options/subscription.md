# Subscription Options

Any implementation of the `ISubscriptionService` interface should make use of the `SubscriptionOptions` configured with the `ServiceBusOptions` to register, or ensure, any subscriptions:

```c#
services.AddServiceBus(builder => {
	builder.Subscription.SubscribeType = SubscribeType.Normal; // default
    builder.Subscription.ConnectionString = "Subscription"; // default

    // add subscription to message types directly; else below options on builder
    builder.Subscription.MessageTypes.Add(messageType);

    // using type
    builder.AddSubscription(typeof(Event1));
    builder.AddSubscription(typeof(Event2));

    // using a full type name
    builder.AddSubscription(typeof(Event1).FullName);
    builder.AddSubscription(typeof(Event2).FullName);

    // using a generic
    builder.AddSubscription<Event1>();
    builder.AddSubscription<Event2>();
});
```

And the JSON configuration structure:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "Subscription": {
        "SubscribeType": "Normal",
        "ConnectionStringName": "connection-string",
        "MessageTypes": [
          "message-type-a",
          "message-type-b"
        ]
      }
    }
  }
}
```

## Options

| Option | Default	| Description | 
| --- | --- | --- |
| `ConnectionStringName`	 | Subscription | The name of the `ConnectionString` to use to connect to the subscription store. |
| `SubscribeType`	| Normal | Indicates subscriptions are dealt with: <br/>- `Normal` is the ***default*** and will subscribe to the given message type(s) if they have not been subscribed to yet.<br/>- `Ensure` will check to see that the subscription exists and if not will throw an `ApplicationException`.<br/>- `Ignore` will simply ignore the subscription request. |

Should the endpoint be configured as a worker no new subscriptions will be registered against the endpoint since any published events should be subscribed to only by the distributor endpoint.  When using a broker all the endpoints feed off the same work queue uri and any of the endpoints could create the subscription.

When moving to a non-development environment it is recommended that you make use of the `Ensure` option for the `SubscribeType`.