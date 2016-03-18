---
title: TransportMessageConfigurator
layout: api
---
# TransportMessageConfigurator

## Properties

### Headers

~~~ c#
public List<TransportHeader> Headers { get; private set; }
~~~

Contains the list of name/value pair transport headers.  Whenever a `TransportMessageReceived` is set 

## Methods

### Local

~~~ c#
public TransportMessageConfigurator Local()
~~~

This sets the `RecipientInboxWorkQueueUri` tot he local inbox work queue uri.  If there is no inbox an expection is thrown.

### Reply

~~~ c#
public TransportMessageConfigurator Reply()
~~~

This sets the `RecipientInboxWorkQueueUri` to the `SenderInboxWorkQueueUri` of the transport message that was received.  This method, therefore, is only available in a [HandlerContext].  If no transport message received instance is available or if the received transport message does not have a `SenderInboxWorkQueueUri` property (the sender has no inbox) an exception is thrown.

### WithRecipient

~~~ c#
public TransportMessageConfigurator WithRecipient(IQueue queue)
public TransportMessageConfigurator WithRecipient(Uri uri)
public TransportMessageConfigurator WithRecipient(string uri)
~~~

This sets the `RecipientInboxWorkQueueUri` explicitly and no routing will be applied.

### WithCorrelationId

~~~ c#
public TransportMessageConfigurator WithCorrelationId(string correlationId)
~~~

Sets the correlation if for the message.

### Defer

~~~ c#
public TransportMessageConfigurator Defer(DateTime ignoreTillDate)
~~~

Ignores the [TransportMessage] until the given date/time has been reached.  The [TransportMessage] is sent immediately and it is up to the receiving endpoint to decide how to defer it.  It is recommended that you configure a deferred queue:

~~~ xml
	<serviceBus>
		<inbox
		  workQueueUri="msmq://./pubsub-client-inbox-work"
		  deferredQueueUri="msmq://./pubsub-client-inbox-deferred" <!-- THE DEFERRED QUEUE STORE -->
		  errorQueueUri="msmq://./shuttle-samples-error" />
	</serviceBus>
~~~

Without a dedicated deferred queue the work queue will contain the deferred message.  This may slow down processing as a deferred message is not regarded as work.  If the queue contains too many deferred messages the queue processing will become very slow and unresponsive.

### TransportMessageReceived

~~~ c#
public void TransportMessageReceived(TransportMessage transportMessageReceived)
~~~

This will be called by shuttle-esb directly.  The call will add all the received headers to the configurator headers and set the correlation id to the correlation id of the received transport message.

### HasTransportMessageReceived

~~~ c#
public bool HasTransportMessageReceived
~~~

Returns `true` if a `TransportMessageReceived` has been set for the [MessageSender] else `false`.

### TransportMessage

~~~ c#
public TransportMessage TransportMessage(IServiceBusConfiguration configuration)
~~~

This returns a new [TransportMessage] that contains the configured properties.

[MessageSender]: {{ site.baseurl }}/message-sender
[HandlerContext]: {{ site.baseurl }}/handler-context
[TransportMessage]: {{ site.baseurl }}/transport-message
