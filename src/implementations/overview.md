# Implementations

These packages are those implementing the queue interfaces `IQueue` and `IQueueFactory`, subscription service implementations of `ISubscriptionService`, and then the `IIdempotenceService` implementations.

## Queues

The convention for queue URIs is `scheme://configuration-name/queue-name` and the `scheme` represents a unique name for the `IQueue` implementation.  The `scheme` and `configuration-name` (represented by the URI's `Host` property) should always be lowercase as creating a `new Uri(uriString)` forces the scheme and host to lowercase.

Each `configuration` is a named set of options and would contain all the values required to communicate with the `queue` as well as any other bits that may be of interest.

The typical JSON settings structure for a queue implementation would follow the following convetion:

```json
{
  "Shuttle": {
    "ImplementationName": {
      "configuration-name": {
          "OptionA": "value-a"
      }
    }
  }
}
```

## Streams

Stream implementations also implement the same interfaces as queues except that the `IQueue.IsStream` returns `true` which allows the service bus to handle exceptions differently.