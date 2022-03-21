# Message Distribution

It is conceivable that an endpoint can start falling behind with its processing if it receives too much work.  In such cases it may be changed to distribute messages to worker nodes.

![Message Distribution Image](/images/message-distribution.png)

An endpoint will automatically distribute messages to workers if it receives a worker availability message.  An endpoint can be configured to only distribute messages, and therefore not process any messages itself, by setting the `distribute` attribute of the `inbox` configuration tag to `true`.

Since message distribution is integrated into the inbox processing the same endpoint simply needs to be installed aa many times as required on different machines as workers.  The endpoint that you would like to have messages distributed on would require a control inbox configuration since all Shuttle messages should be processed without waiting in a queue like the inbox proper behind potentially thousands of messages.  Each worker is identified as such in its configuration and the control inbox of the endpoint performing the distribution is required:

```xml
<configuration>
   <configSections>
      <section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
   </configSections>

   <serviceBus>
      <control 
          workQueueUri="msmq://./control-inbox-work" 
          errorQueueUri="msmq://./shuttle-error"/>
      <inbox 
          distribute="true"
          workQueueUri="msmq://./inbox-work" 
          errorQueueUri="msmq://./shuttle-error"/>
   </serviceBus>
</configuration>
```

Any endpoint that receives messages can be configured to include message distribution.

You then install as many workers as you require on as many machines as you want to and configure them to talk to a distributor.  The physical distributor along with all the related workers form the logical endpoint for a message.  The worker configuration is as follows:

```xml
<configuration>
   <configSections>
      <section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
   </configSections>

   <serviceBus>
      <worker
         distributorControlWorkQueueUri="msmq:///control-inbox=work" />
      <inbox
         workQueueUri="msmq://./workerN-inbox-work"
         errorQueueUri="msmq://./shuttle-error"
         threadCount="15">
      </inbox>
   </serviceBus>
</configuration>
```

As soon as the application configuration file contains the **worker** tag each thread that goes idle will send a message to the distributor to indicate that a thread has become available to perform word.  The distributor will then send one or more message for each available thread.  The number of messages sent for each availability message is configured using the `distributeSendCount` attribute of the `inbox` tag.

## Message Distribution Exceptions

Some queueing technologies do not require message distribution.  Instead of a worker another instance of the endpoint can consume the same input queue.  This mechanism applies to brokers.  Since brokers manage queues centrally the messages are consumed via consumers typically running per thread.  Where the consumers originates does not matter so the queue can be consumed from various processes.

The broker style differes from something like Msmq or Sql-based queues where the message-handling is managed by the process hosting the thread-consumers.  Here `process-A` would not be aware of which messages are being consumed by `process-B` leading to one *stealing* messages from the other.

