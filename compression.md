---
title: Compression
layout: api
---
# ICompressionAlgorithm

There may be zero or more implementations of the `ICompressionAlgorithm` registered at any one time.

The following algorithms are included out-of-the-box:

- `DeflateCompressionAlgorithm`
- `GZipCompressionAlgorithm`

All messages that are outgoing will be compressed using the algorithm name specified in the `serviceBus` configuration section (empty for no compression):

```xml
  <serviceBus
    compressionAlgorithm="">
```

## Properties

### Name

Returns the name of the compression algorithm.  The name should be unique and it used to find the algorithm used in the `TransportMessage.CompressionAlgorithm` property.

## Methods

### Compress

``` c#
byte[] Compress(byte[] bytes);
```

Returns an array of bytes representing the compressed input.

### Decompress

``` c#
byte[] Decompress(byte[] bytes);
```

Returns an array of bytes representing the decompressed input.
