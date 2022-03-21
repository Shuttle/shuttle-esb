# File System

```
PM> Install-Package Shuttle.Esb.FileMQ
```

File-based queue implementation for use with Shuttle.Esb.

This queue implementation is not intended to be used in a production environment other than for backing up / copying queue messages.

# FileMQ

This `IQueue` implementation makes use of a folder as a queue with the messages saved as file.  It is provided mainly as a backup mechanism.

## Configuration

The queue configuration is part of the specified uri, e.g.:

``` xml
    <inbox
      workQueueUri="filemq://{directory-path}"
	  .
	  .
	  .
    />
```
