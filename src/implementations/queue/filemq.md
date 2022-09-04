# File System

```
PM> Install-Package Shuttle.Esb.FileMQ
```

File-based queue implementation for use with Shuttle.Esb.

This queue implementation is not intended to be used in a production environment other than for backing up / copying queue messages.

# FileMQ

This `IQueue` implementation makes use of a folder as a queue with the messages saved as file.  It is provided mainly as a backup mechanism.

## Configuration

The URI structure is `filemq://configuration-name/queue-name`.

```c#
services.AddFileQueue(builder =>
{
    builder.AddOptions("local", new FileQueueOptions
    {
        Path = $"{Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".."))}\\queues\\"
    });
});
```

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "FileMQ": {
      "local": {
        "Path": "c:\\queues\\",
      }
    }
  }
}
```

## Options

| Segment / Argument | Default | Description |
| --- | --- | --- | 
| Path | | The root folder that will contain a queue name as a sub-folder. |
