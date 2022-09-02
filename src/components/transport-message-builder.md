# TransportMessageBuilder

## Properties

### Headers

``` c#
public List<TransportHeader> Headers { get; private set; }
```

Contains the list of name/value pair transport headers.  Whenever a `TransportMessageReceived` is available the received transport headers are also included.

## Methods

### Local

``` c#
public TransportMessageBuilder Local();
```

This sets the `RecipientInboxWorkQueueUri` tot he local inbox work queue uri.  If there is no inbox an expection is thrown.

### Reply

``` c#
public TransportMessageBuilder Reply();
```

This sets the `RecipientInboxWorkQueueUri` to the `SenderInboxWorkQueueUri` of the transport message that was received.  This method, therefore, is only available in a [HandlerContext].  If no transport message received instance is available or if the received transport message does not have a `SenderInboxWorkQueueUri` property (the sender has no inbox) an exception is thrown.

### WithRecipient

``` c#
public TransportMessageBuilder WithRecipient(IQueue queue);
public TransportMessageBuilder WithRecipient(Uri uri);
public TransportMessageBuilder WithRecipient(string uri);
```

This sets the `RecipientInboxWorkQueueUri` explicitly and no routing will be applied.

### WithCorrelationId

``` c#
public TransportMessageBuilder WithCorrelationId(string correlationId);
```

Sets the correlation id for the message.

### Defer

``` c#
public TransportMessageBuilder Defer(DateTime ignoreTillDate);
```

Ignores the `TransportMessage` until the given date/time has been reached.  The `TransportMessage` is sent immediately and it is up to the receiving endpoint to decide how to defer it.  It is recommended that you configure a deferred queue.

Without a dedicated deferred queue the work queue will contain the deferred message.  This may slow down processing as a deferred message is not regarded as work.  If the queue contains too many deferred messages the queue processing will become very slow and unresponsive.

### WillExpire

``` c#
public TransportMessageBuilder WillExpire(DateTime expiryDate);
```

The message will only remain valid until this date is reached.  After this is will not be processed but rather immediately acknowledged.  The queuing mechanism may also, should it support message expiry, remove the message internally from the queue.

### WithPriority

``` c#
public TransportMessageBuilder WithPriority(int priority);
```

### WithEncryption

``` c#
public TransportMessageBuilder WithEncryption(string encryption);
```

### WithCompression

``` c#
public TransportMessageBuilder WithCompression(string compression);
```

### HasRecipient

``` c#
public bool HasRecipient { get; }
```

Returns `true` if the transport message has a `RecipientInboxWorkQueueUri`; else `false`.
