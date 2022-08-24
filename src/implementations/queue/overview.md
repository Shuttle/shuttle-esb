# Queues

The convention for queue URIs is `scheme://configuration-name/queue-name` and the `scheme` represents a unique name for the `IQueue` implementation.

Each `configuration` is a named set of options and would contain all the values required to communicate with the `queue` as well as any other bits that may be of interest.

The typical JSON settings structure for a queue implementation would follow the following convetion:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "ImplementationName": {
        "configuration-name": {
            "OptionA": "value-a"
        }
      }
    }
  }
}
```