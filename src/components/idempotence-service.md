# Idempotence Service

An implementation of the `IIdempotenceService` interface is responsible for ensuring that message remain idempotent on a technical level.  This means that if, by some edge case, a message happens to be duplicated then only one instance of the message will be processed.  This is done by keeping track of which message ids have been processed.

In addition to this the idempotence service also defers message sending when message are sent (or published) within a transaction.

## Methods

### ProcessingStatus

``` c#
ProcessingStatus ProcessingStatus(TransportMessage transportMessage);
```

This method must return the `ProcessingStatus` of the given `TransportMessage`:

- Returns `ProcessingStatus.Ignore` if the message has been **processed** completely and also if it currently being processed by another consumer.
- Returns `ProcessingStatus.MessageHandled` if the message has already been handled.  There may be deferred messages that need to be sent.
- Returns `ProcessingStatus.Assigned` if this message is assigned for initial processing.

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

### MessageHandled

``` c#
void MessageHandled(TransportMessage transportMessage);
```

Once the message has been successfully handled this method is called to mark the message as handled in the store.