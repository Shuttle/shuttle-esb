---
title: Deferred Message Concepts
layout: api
---
# Deferred Messages

A deferred message is one that cannot/should not be immediately processed.  There are two ways in which a message will be deferred.  The first is when the processing of a message fails and the inbox is configured, as follows, to wait between retries:

``` xml
<configuration>
    <configSections>
        <section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
    </configSections>

    <serviceBus>
        <inbox 
            workQueueUri="msmq://./shuttle-server-work" 
            deferredQueueUri="msmq://./shuttle-server-deferred" 
            errorQueueUri="msmq://./shuttle-error"
            durationToIgnoreOnFailure="10s,30s,1m" />
    </serviceBus>
</configuration>
```

The first failure will cause the message to wait for 10 seconds before being tried again.

If there is no deferred queue the message is simply returned to the work queue.  However, the work queue is designed such that processing can occur as quickly as possible so having deferred messages in the work queue will result in some serious queue thrashing so this should be avoided for all but the simplest samples.

## Deferred Queue

A deferred queue may be configured for an inbox:

``` xml
<configuration>
    <configSections>
        <section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
    </configSections>

    <serviceBus>
        <inbox 
            workQueueUri="msmq://./shuttle-server-work" 
            deferredQueueUri="msmq://./shuttle-server-deferred" 
            errorQueueUri="msmq://./shuttle-error"/>
    </serviceBus>
</configuration>
```

An important point to remember is that a deferred queue belongs to the particular instance of the endpoint.  **A deferred queue should *never* be shared**.  This means that you should never find a deferred queue uri in more than one configuration.  Even when one may have the same work queue uri across distributed endpoints in the case of a brokered queue (such as RabbitMQ) you would *still not* use the same deferred queue.  It all has to do with how a deferred queue is processed.

The deferred queue is processed in single iterations.  It is processed when the endpoint starts up and then only again when required.

Messages never route directly to a deferred queue.  Instead they always go to the work queue and if the work queue sees finds a future `IgnoreTillDate` in the [TransportMessage] then it is moved to the deferred queue and the next date to process the deferred queue is set to this `IngoreTillDate` if it is less than the next deferred queue process date.

[TransportMessage]: {{ "/transport-message" | relative_url }}
