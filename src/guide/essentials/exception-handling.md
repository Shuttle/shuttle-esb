# Exception Handling

When an exception occurs within a pipeline an `OnPipelineException` event is raised on the pipeline and any observers that have hooked onto the event will be called:

``` c#
public class ReceiveExceptionObserver : IPipelineObserver<OnPipelineException>
{
    public void Execute(OnPipelineException pipelineEvent)
    {
        // set by calling MarkExceptionHandled
        if (pipelineEvent.Pipeline.ExceptionHandled) 
        {
            return;
        }

        // sets ExceptionHandled to true
        pipelineEvent.Pipeline.MarkExceptionHandled(); 

        // prevents further processing of the pipeline
        pipelineEvent.Pipeline.Abort(); 
    }
}
```

Typically you would not respond to the `OnPipelineException` event but you need to be cognizant of how it affects the message processing.

Exception handling differs between queues and streams.

## Queues

The more interesting bit is that the first order of business is to determine whether or not to retry the message.  If the exception is of type `UnrecoverableHandlerException` the message will not be retried and will be immediately moved to the `ErrorQueue`.  This means that when you are able to determine that the message will never be processed correctly, but you do not want to discard it outright, you can throw this exception and the message will be moved directly to the `ErrorQueue`.  There is also an option to set the `IHandlerContext.ExceptionHandling` to `ExceptionHandling.Poison` which will also move the message directly to the `ErrorQueue`.  In instances where no `ErrorQueue` has been defined the message will simply be released.  In most `IQueue` implementations this will leave the message at the head of the queue and it will immediately become available for processing again.  This would lead to your queue processing being blocked by this message which is, of course, less than ideal and the message should be handled or acknowledged/ignored in some way.

If the exception is not of type `UnrecoverableHandlerException` the implementation of the `IServiceBusPolicy` is used to invoke the relevant policy method.  The `DefaultServiceBusPolicy` makes use of the `ServiceBusOptions` to determine the `MaximumFailureCount`.  Should the number of failed messages still be within this count the message will be retried and duration to wait until the next retry is determined by using the `DurationToIgnoreOnFailure` value.

Should the message be retried the exception message is added to the `FailureMessages` collection of the `TransportMessage` and the transport message is re-enqueued, moving it to the end of the queue and the `IgnoreTillDate` property of the `TransportMessage` is also set to the duration to wait before retrying.  It is also possible to set the `IHandlerContext.ExceptionHandling` to `ExceptionHandling.Retry` to explicitly retry the message when you encounter an exception.

**Note**: you may experience queue thrashing if you do not have a `DeferredQueue` configured for your inbox and mesasges are retried.  When the inbox processor dequeues the message it determines whether it can be processed by checking the `IgnoreTillDate` value.  Should the  `IgnoreTillDate` be in the future (deferred) it is moved to the `DeferredQueue` if there is one; else it is simply re-enqueued in the inbox which moves it to the back of the queue.  Since the inbox is responsible for processing messages as quickly as possible this processing will continue until the message again becomes available for processing.  Given that there is a message to process in the inbox no backing-off will occur.  Please always use a deferred queue for anything that will be executed in a production environment.

## Streams

Exception handling for streams is mostly the same as for queues except that no retries are permitted.  The reason being that a stream is not specific to any specific consumer and producing another message will immediately result in duplicate processing.

Message sent to streams also may not be deferred.