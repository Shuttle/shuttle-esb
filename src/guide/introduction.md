# The Broad Strokes

Shuttle.Esb supports processing from both queues and streams.

All processing is performed on messages (serialized objects) that are received from a queue and by finding a message handler that can handle the type of the message (`instance.GetType().Name`).  Typically messages are sent to a queue to be processed and this combination of queue and the `ServiceBus` instance that performs the processing is referred to as an *endpoint*:

![Endpoint Image](/images/endpoint.png)

It is important to note that *not* every `ServiceBus` instance will necessarily process messages from an inbox.  This happens when the instance is a producer of messages only.  An example may be a `web-api` that receives integration requests that are then sent to a relevant endpoint queue as a `command` message.

Similarly, not every queue is going to be consumed by a `ServiceBus` instance.  An example of this is the error queue where poison messages are routed to.  These queues have to be managed out-of-band to determine the cause of the failure before moving the messages back to the inbox queue for another round of processing.

## Streams

Streams are handled slightly differently to queues when it comes to exception handling.

When processing messages successfully from a **queue** a message is retrieved, handled, and then acknowledged.  Once acknowledge, a message is removed from the queue.  When a message fails it is **re-queued** for re-processing later on.  It may also be moved to a deferred queue, if configured, before being moved back to the inbox work queue later on.  The reason it is re-queued is that simply releasing the message would depend on the queuing mechanism to decide what to do with the message and it may remain at the head of the queue resulting in blocking any further movement on the consumer.

In contrast to queues, consumers do not ever remove messages from the stream.  Streaming is a read-only operation for consumers.  A stream is a log of messages that typically have a retention policy which may be age or size, or a combination thereof.  As long a the messages are available in a stream any consumer may start reading from the stream at from any point, although the start/oldest/tail of the stream or the end/newest/head of the stream would be typical starting positions.  Each consumer has an offset which indicates where they are in the stream and once a message is acknowledged this offest is moved ahead to indicate the new position.  It is up to the consuming application to decide how to handle exceptions.  A message cannot be re-queued as that would result in a duplicate that would be picked up by other consumers are well.  If a poison message is not dealt with the consumer will remain blocked.  One way to deal with a poison message is to make use of an error (poison) queue.  There is no straghtforward answer to dealing with these error as it will depend on the type of error and use-case.  There are also some [exception handling](/guide/essentials/exception-handling) indicators available on the `HandlerContext`.

## Threading / Number of consumers

For queues there is no real restriction on how many consumers one can create per queue.  Each thread would result in a consumer.  This is not the case for streams.  Since a conmsumer moves forward along the stream/topic it does not make sense for another thread on the same consumer to be reading messages related to the same stream/topic from another location.  In this case the service bus endpoint is one logical unit with, say, two threads.  In order to perform parallel processing one would need the stream to be partitioned.  This results in different logicial/physical streams that may be processed independently.  In this case the number of consumers may be equal to, or less than, the number of partitions.  Having more consumers than partitions would result in the extra consumers remaining idle as they will never receive any messages.