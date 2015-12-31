---
title: Serializer
layout: api
---
# ISerializer

The `shuttle-esb-core` package makes use of the `shuttle-core-infrastructure` package's `ISerializer` interface for serializing any objects.

For more information you can [visit the relevant documentation page](http://shuttle.github.io/shuttle-core/overview-serializer/).

## Caveats

It is recommended that you have a look at the duplicate class name caveat described in above documentation so please remember to explicitly specify you message types.

Another point is that the `shuttle-esb-core` types used are not specified explicitly in order to maintain backward compatability.  Please do not name any of your types as follows:

- TransportMessage
- TransportHeader
- AvailableWorker
- WorkerStartedEvent
