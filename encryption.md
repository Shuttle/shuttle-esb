---
title: Encryption
layout: api
---
# IEncryptionAlgorithm

There may be zero or more implementations of the `IEncryptionAlgorithm` registered at any one time.

The following algorithms are included out-of-the-box:

- `TripleDesEncryptionAlgorithm`

All messages that are outgoing will be encrypted using the algorithm name specified in the `serviceBus` configuration section (empty for no encryption):

```xml
  <serviceBus
    encryptionAlgorithm="">
```

## Properties

### Name

Returns the name of the encryption algorithm.  The name should be unique and it used to find the algorithm used in the `TransportMessage.EncryptionAlgorithm` property.

## Methods

### Encrypt

``` c#
byte[] Encrypt(byte[] bytes);
```

Returns an array of bytes representing the encrypted input.

### Decrypt

``` c#
byte[] Decrypt(byte[] bytes);
```

Returns an array of bytes representing the unencrypted input.
