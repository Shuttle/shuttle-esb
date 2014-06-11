---
title: Thread Activity Factory
layout: api
---
# IThreadActivityFactory

An implementation of the `IThreadActivityFactory` interface is used to create `ThreadActivity` instances that can be used to manage how long a thread sleeps when it is not active.  The `ThreadActivity` instance also responds immediately when the thread is de-activated.

The `DefaultThreadActivityFactory` implementation should suffice.

## Methods

### CreateInboxThreadActivity

``` c#
IThreadActivity CreateInboxThreadActivity(IServiceBus bus);
```

Returns a `ThreadActivity` object configured using the `InboxConfiguration`.

### CreateControlInboxThreadActivity

``` c#
IThreadActivity CreateControlInboxThreadActivity(IServiceBus bus);
```

Returns a `ThreadActivity` object configured using the `ControlInboxConfiguration`.

### CreateOutboxThreadActivity

``` c#
IThreadActivity CreateOutboxThreadActivity(IServiceBus bus);
```

Returns a `ThreadActivity` object configured using the `OutboxConfiguration`.


