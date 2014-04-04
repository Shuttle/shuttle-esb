using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ReceiveExceptionObserver :
		IPipelineObserver<OnPipelineException>
	{
		/* 
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

					var action = bus.Configuration.Policy.EvaluateMessageHandlingFailure(pipelineEvent);

					transportMessage.RegisterFailure(pipelineEvent.Pipeline.Exception.CompactMessages(), action.TimeSpanToIgnoreRetriedMessage);

					using (var stream = bus.Configuration.Serializer.Serialize(transportMessage))
					{
						var handler = pipelineEvent.GetMessageHandler();
						var handlerFullTypeName = handler != null ? handler.GetType().FullName : "(handler is null)";
						var currentRetryCount = transportMessage.FailureMessages.Count;

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
					}

					pipelineEvent.GetWorkQueue().Acknowledge(transportMessage.MessageId);
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