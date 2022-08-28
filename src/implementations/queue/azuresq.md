# Azure Storage Queues

```
PM> Install-Package Shuttle.Esb.AzureStorageQueues
```

In order to make use of the `AzureStorageQueue` you will need access to an Azure Storage account or use the [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite) emulator for local Azure Storage development.

You may want to take a look at how to [get started with Azure Queue storage using .NET](https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues?tabs=dotnet).

## Configuration

The URI structure is `azuresq://configuration-name/queue-name`.

```c#
services.AddAzureStorageQueues(builder =>
{
    var azureStorageQueueOptions = new AzureStorageQueueOptions
    {
        ConnectionString = "UseDevelopmentStorage=true",
        MaxMessages = 20,
        VisibilityTimeout = null
    };

    azureStorageQueueOptions.Configure += (sender, args) =>
    {
        Console.WriteLine($"[event] : Configure / Uri = '{((IQueue)sender).Uri}'");
    };

    builder.AddOptions("azure", azureStorageQueueOptions);
});
```

The `Configure` event `args` arugment exposes the `QueueClientOptions` directly for any specific options that need to be set.

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "AzureStorageQueues": {
      "azure": {
        "ConnectionString": "UseDevelopmentStorage=true",
        "MaxMessages": 32,
        "VisibilityTimeout": "00:00:30"
      }
    }
  }
}
```

## Options

| Segment / Argument | Default | Description |
| --- | --- | --- | 
| ConnectionString | | The Azure Storage Queue endpoint to connect to. |
| MaxMessages | 32 | Specifies the number of messages to fetch from the queue. |
| VisibilityTimeout | `null` | | The message visibility timeout that will be used for messages that fail processing. |
