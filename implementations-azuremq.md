---
title: AzureMQ
layout: api
---
# AzureStorageQueue

In order to make use of the `AzureStorageQueue` you will need access to an Azure Storage account or [use the Azurite emulator for local Azure Storage development](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite) for local development.

You may want to take a look at how to [get started with Azure Queue storage using .NET](https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues?tabs=dotnet).

## Configuration

The queue configuration is part of the specified uri, e.g.:

``` xml
<inbox
    workQueueUri="azuremq://connection-name/queue-name?maxMessages=15"
    .
    .
    .
/>
```

| Segment / Argument | Default | Description |
| --- | --- | --- | 
| connection-name | required | Will be resolved by an `IAzureConfiguration` implementation (*see below*). |
| queue-name | required | The name of queue to connect to. |
| maxMessages | 1 | Specifies the number of messages to fetch from the queue. |

## IAzureConfiguration

```c#
string GetConnectionString(string name);
```

Should return the Azure Storage connection string to use.  For local `azurite` development this would be `UseDevelopmentStorage=true`.

## DefaultAzureConfiguration

The `DefaultAzureConfiguration` instance implementing the `IAzureConfiguration` interface will be registered using the [container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#Bootstrapping).  If you wish to override the configuration you should register your instance before calling the `ServiceBus.Register()` method.

This implementation will return the `appSetting` value for the specified `connection-name` as the Azure Storage conenction string:

```xml
<appSettings>
    <add key="azure" value="UseDevelopmentStorage=true" />
</appSettings>

<inbox
    workQueueUri="azuremq://azure/server-inbox-work-queue"
    .
    .
    .
/>
```
