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
			var state = pipelineEvent.Pipeline.State;
			var bus = state.GetServiceBus();

			bus.Events.OnBeforePipelineExceptionHandled(this, new PipelineExceptionEventArgs(pipelineEvent.Pipeline));

			try
			{
				if (pipelineEvent.Pipeline.ExceptionHandled)
				{
					return;
				}

				try
				{
					var transportMessage = state.GetTransportMessage();

					if (transportMessage == null)
					{
						return;
					}

					var action = bus.Configuration.Policy.EvaluateMessageHandlingFailure(pipelineEvent);

					transportMessage.RegisterFailure(pipelineEvent.Pipeline.Exception.CompactMessages(), action.TimeSpanToIgnoreRetriedMessage);

					using (var stream = bus.Configuration.Serializer.Serialize(transportMessage))
					{
						var handler = state.GetMessageHandler();
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
							                          state.GetMaximumFailureCount()));

							state.GetWorkQueue().Enqueue(transportMessage.MessageId, stream);
						}
						else
						{
							Log.For(this)
							   .Error(string.Format(ESBResources.MessageHandlerExceptionFailure,
							                        handlerFullTypeName,
							                        pipelineEvent.Pipeline.Exception.CompactMessages(),
							                        transportMessage.MessageType,
							                        transportMessage.MessageId,
							                        state.GetMaximumFailureCount(),
							                        state.GetErrorQueue().Uri));

							state.GetErrorQueue().Enqueue(transportMessage.MessageId, stream);
						}
					}

					state.GetWorkQueue().Acknowledge(transportMessage.MessageId);
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