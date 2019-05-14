---
title: Exception Handling
layout: api
---
# Overview

When an exception occurs within a pipeline an `OnPipelineException` event is raised on the pipeline and any observers that have hooked onto the event will be called:

``` c#
public class ReceiveExceptionObserver : IPipelineObserver<OnPipelineException>
{
    public void Execute(OnPipelineException pipelineEvent)
    {
        if (pipelineEvent.Pipeline.ExceptionHandled) // <-- set by calling MarkExceptionHandled
        {
            return;
        }

        pipelineEvent.Pipeline.MarkExceptionHandled(); // <-- sets ExceptionHandled to true
        pipelineEvent.Pipeline.Abort(); // <-- prevents further processing of the pipeline
    }
}
```

Typically you would not respond to the `OnPipelineException` event but you need to be cognizant of how it affects the message processing.

The more interesting bit is that the first order of business is to determine whether or not to retry the message.  If the exception is of type `UnrecoverableHandlerException` the message will not be retried and will be immediately moved to the `ErrorQueue`.  This means that when you are able to determine that the message will never be processed correctly, but you do not want to discard it outright, you can throw this exception and the message will be moved directly to the `ErrorQueue`.

If the exception is not of type `UnrecoverableHandlerException` the implementation of the `IServiceBusPolicy` on the `ServiceBusConfiguration` is used to invoke the relevant method.  The `DefaultServiceBusPolicy` makes use of the `serviceBus` settings  in the application configuration file to determine the 'maximumFailureCount'.  Should the number of failed messages still be within this count the message will be retried and duration to wait until the next retry is determined by using the `durationToIgnoreOnFailure` value.

Should the message be retried the exception message is added to the `FailureMessages` collection of the [TransportMessage] and the transport message is re-enqueued, moving it to the end of the queue and the `IgnoreTillDate` property of the [TransportMessage] is also set to the duration to wait before retrying. 

**Note**: this is where you may experience some serious queue thrashing if you do not have a `DeferredQueue` configured for your inbox.  When the inbox processor dequeues the message it check to see whether it can be processed by checking the `IgnoreTillDate` value.  If it cannot be processed it is moved to the `DeferredQueue` if there is one; else it is simply re-enqueued in the inbox.  Since the inbox is responsible for processing messages as quickly as possible this processing will continue until the message becomes available for processing.  Given that there is a message to process in the inbox no backing-off will occur.  Please always use a deferred queue for anything that will be executed in a production environment.

[TransportMessage]: {{ "/transport-message" | relative_url }}
