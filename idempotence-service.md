---
title: Idempotence Service
layout: api
---
# Idempotence Service

An implementation of the `IIdempotenceService` interface is responsible for ensuring that message remain idempotent on a technical level.  This means that if, by some edge case, a message happens to be duplicated then only one instance of the message will be processed.  This is done by keeping track of which message ids have been processed.

In addition to this the idempotence service also defers message sending when message are sent (or published) within a transaction.

## Methods

### ShouldProcess

``` c#
bool ShouldProcess(TransportMessage transportMessage);
```

This method must return `false` if the message has previously been processed successfully or if the message is currently being processed; else it should mark the message as being processed and return `true`.

### ProcessingCompleted

``` c#
void ProcessingCompleted(TransportMessage transportMessage);
```

Marks the message as having being processed successfully.

### AddDeferredMessage

``` c#
void AddDeferredMessage(TransportMessage processingTransportMessage, Stream deferredTransportMessageStream);
```

Saves the `deferredTransportMessageStream` against the given `processingTransportMessage` in order for the service bus to perform the actual dispatching of the deferred message after the messae ahs been handled.

### GetDeferredMessages

``` c#
IEnumerable<Stream> GetDeferredMessages(TransportMessage transportMessage);
```

Returns all the streams that were sent during the handling of the given `transportMesage`.

### DeferredMessageSent

``` c#
void DeferredMessageSent(TransportMessage processingTransportMessage, TransportMessage deferredTransportMessage);
```

This method should remove the entry associated with the `deferredTransportMessage` as it has been dispatched.