# TransportMessage

## Properties

### Message

``` c#
public System.Byte[] Message { get; set; }
```

The actual message stream returned from the [Serializer] represented as a byte array.

### MessageReceivedId

``` c#
public System.Guid MessageReceivedId { get; set; }
```

This is the Id of the message that was being processed when the message was sent.  So if message with `MessageId` *ABC123* was received and you sent another message that will have a new `MessageId` of, say, *DEF321* then the `MessageReceivedId` of the new message with `MessageId`: *DEF321* will be *ABC123*.

### MessageId

``` c#
public System.Guid MessageId { get; set; }
```

The unique Id of this message.

### CorrelationId

``` c#
public string CorrelationId { get; set; }
```

The `CorrelationId` is not used by the core Shuttle.Esb and you are free to use it to correlate your messages.

### SenderInboxWorkQueueUri

``` c#
public string SenderInboxWorkQueueUri { get; set; }
```

The `Uri` of the inbox of the endpoint where the message originated.  If the sender did not have an inbox then this value will be empty.

### RecipientInboxWorkQueueUri

``` c#
public string RecipientInboxWorkQueueUri { get; set; }
```

The `Uri` of the inbox of the destination endpoint of this message.

### PrincipalIdentityName

``` c#
public string PrincipalIdentityName { get; set; }
```

The name of `WindowsIdentity` that dispatched the message.  May be *Anonymous*.

### IgnoreTillDate

``` c#
public System.DateTime IgnoreTillDate { get; set; }
```

The message will not be processed while the current date is before this date.

### ExpiryDate

``` c#
public System.DateTime ExpiryDate { get; set; }
```

### Priority

``` c#
public int Priority { get; set; }
```

Determines the date that the message will expire.  It will not be processed after this date.  The default value is `DateTime.MaxValue`..

### SendDate

``` c#
public System.DateTime SendDate { get; set; }
```

The date that the message was sent.

### FailureMessages

``` c#
public List<string> FailureMessages { get; set; }
```

A list of message containing each failure that occurred.

### MessageType

``` c#
public string MessageType { get; set; }
```

The `FullName` of the message type represented by the `Message` property.

### AssemblyQualifiedName

``` c#
public string AssemblyQualifiedName { get; set; }
```

The `AssemblyQualifiedName` of the message type represented by the `Message` property.

### EncryptionAlgorithm

``` c#
public string EncryptionAlgorithm { get; set; }
```

The name of the encryption algorithm used to encrypt the message; else empty.

### CompressionAlgorithm 

``` c#
public string CompressionAlgorithm { get; set; }
```

The name of the compression algorithm used to compress the message; else empty.

### Headers

``` c#
public List<Shuttle.Esb.TransportHeader> Headers { get; set; }
```

An arbitrary list of `TransportHeader` objects that may be used to carry information not contained in the `Message` property.
