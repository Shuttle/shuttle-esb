using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class ReceiveExceptionObserver :
        IPipelineObserver<OnPipelineException>
    {
        /* WITH JOURNAL QUEUE:
         * 
         * If in the 'Read' stage the message is still in the work queue
         * - enqueue in error queue
         * - remove from work queue
         * 
         * If in the 'Handle' stage the message has moved to the journal queue
         * - for retry enqueue in work queue; else enqueue in error queue
         * - remove from journal queue
         * 
         * WITHOUT JOURNAL QUEUE:
         * 
         * If in the 'Read' stage
         * - enqueue in error queue
         * 
         * If in the 'Handle' stage 
         * - for retry enqueue in work queue; else enqueue in error queue
         * 
         */
        public void Execute(OnPipelineException pipelineEvent)
        {
            var bus = pipelineEvent.GetServiceBus();

            bus.Events.OnBeforePipelineExceptionHandled(this, new PipelineExceptionEventArgs(pipelineEvent.Pipeline));

            try
            {
                if (pipelineEvent.Pipeline.ExceptionHandled)
                {
                    return;
                }

                try
                {
                    var transportMessage = pipelineEvent.GetTransportMessage();

                    if (transportMessage == null)
                    {
                        return;
                    }

                    var scope = pipelineEvent.GetTransactionScope();

                    try
                    {
                        if (pipelineEvent.GetHasJournalQueue())
                        {
                            pipelineEvent.GetTransactionScope().Dispose();
                            pipelineEvent.SetTransactionScope(null);

                            scope = bus.Configuration.TransactionScopeFactory.Create(pipelineEvent);
                        }

                        var stream = bus.Configuration.Serializer.Serialize(transportMessage);
                        var handler = pipelineEvent.GetMessageHandler();
                        var handlerFullTypeName = handler != null ? handler.GetType().FullName : "(handler is null)";
                        var currentRetryCount = transportMessage.FailureMessages.Count;

                        var action = bus.Configuration.Policy.EvaluateMessageHandlingFailure(pipelineEvent);

                        transportMessage.RegisterFailure(pipelineEvent.Pipeline.Exception.CompactMessages(), action.TimeSpanToIgnoreRetriedMessage);

                        var retry = pipelineEvent.Pipeline.StageName.Equals("Handle")
                                    &&
                                    !(pipelineEvent.Pipeline.Exception is UnrecoverableHandlerException)
                                    &&
                                    action.Retry;

                        if (retry)
                        {
                            Log.For(this)
                                .Warning(string.Format(ESBResources.MessageHandlerExceptionWillRetry,
                                                       handlerFullTypeName,
                                                       pipelineEvent.Pipeline.Exception.CompactMessages(),
                                                       transportMessage.MessageType,
                                                       transportMessage.MessageId,
                                                       currentRetryCount,
                                                       pipelineEvent.GetMaximumFailureCount()));

                            pipelineEvent.GetWorkQueue().Enqueue(transportMessage.MessageId, stream);
                        }
                        else
                        {
                            Log.For(this)
                                .Error(string.Format(ESBResources.MessageHandlerExceptionFailure,
                                                     handlerFullTypeName,
                                                     pipelineEvent.Pipeline.Exception.CompactMessages(),
                                                     transportMessage.MessageType,
                                                     transportMessage.MessageId,
                                                     pipelineEvent.GetMaximumFailureCount(),
                                                     pipelineEvent.GetErrorQueue().Uri));

                            pipelineEvent.GetErrorQueue().Enqueue(transportMessage.MessageId, stream);
                        }

                        if (pipelineEvent.GetHasJournalQueue())
                        {
                            if (pipelineEvent.Pipeline.StageName.Equals("Handle"))
                            {
                                var journal = pipelineEvent.GetJournalQueue();

                                if (journal != null)
                                {
                                    journal.Remove(transportMessage.MessageId);
                                }
                            }
                            else
                            {
                                pipelineEvent.GetWorkQueue().Remove(transportMessage.MessageId);
                            }
                        }

                        scope.Complete();
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }
                finally
                {
                    pipelineEvent.Pipeline.MarkExceptionHandled();
                    bus.Events.OnAfterPipelineExceptionHandled(this,
                                                               new PipelineExceptionEventArgs(pipelineEvent.Pipeline));
                }
            }
            finally
            {
                pipelineEvent.Pipeline.Abort();
            }
        }
    }
}