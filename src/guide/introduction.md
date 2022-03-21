# The Broad Strokes

All processing is performed on messages (serialized objects) that are received from a queue and then finding a message handler that can handle the type of the message (`instance.GetType().Name`).  Typically messages are sent to a queue to be processed and this combination of queue and the `ServiceBus` instance that performs the processing is referred to as an *endpoint*:

![Endpoint Image](/images/endpoint.png)

It is important to note that *not* every `ServiceBus` instance will process messages from an inbox queue.  This happens when the instance is a producer of messages only.  An example may be a `web-api` that receives integration requests that are then sent to a relevant endpoint queue as a `command` message.

Similarly, not every queue is going to be consumed by a `ServiceBus` instance.  An example of this is the error queue where poison messages are routed to.  These queues have to be managed out-of-band to determine the cause of the failure before moving the messages back to the inbox queue for another round of processing.