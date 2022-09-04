# Queue Factory Service

An implementation of the `IQueueFactoryService` interface is used to manage the queue factories used in Shuttle.Esb.

The queue factory service should not be swapped out for your own implementation as it is integral to the functioning of Shuttle.Esb and the default implementation should suffice.

## Methods

### Get

``` c#
IQueueFactory Get(string scheme);
IQueueFactory Get(Uri uri);
```

The method will return an instance of the queue factory registered for the requested `scheme` in the URI.

### Factories

``` c#
IEnumerable<IQueueFactory> Factories();
```

Returns the `IQueueFactory` implementations that the queue factory service is aware of.

### Register

``` c#
void Register(IQueueFactory queueFactory);
```

Use this method to explicitly register a queue factory instance.

### Contains

``` c#
bool Contains(string scheme);
```

This method determines whether the queue factory service has a queue factory registered for the given scheme.
