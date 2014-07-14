using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ReceiveExceptionObserver :
		IPipelineObserver<OnPipelineException>
	{
		private readonly ILog _log;

		public ReceiveExceptionObserver()
		{
			_log = Log.For(this);
		}

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
					var transportMessageStream = state.GetTransportMessageStream();
					var receivedMessage = state.GetReceivedMessage();

					if (transportMessage == null)
					{
						if (receivedMessage != null && transportMessageStream != null)
						{
							state.GetWorkQueue().Release(receivedMessage.AcknowledgementToken);

							_log.Error(string.Format(ESBResources.ReceivePipelineExceptionMessageReleased, pipelineEvent.Pipeline.Exception.AllMessages()));
						}
						else
						{
							_log.Error(string.Format(ESBResources.ReceivePipelineExceptionMessageNotReceived, pipelineEvent.Pipeline.Exception.AllMessages()));
						}

						return;
					}

					var action = bus.Configuration.Policy.EvaluateMessageHandlingFailure(pipelineEvent);

					transportMessage.RegisterFailure(pipelineEvent.Pipeline.Exception.AllMessages(),
					                                 action.TimeSpanToIgnoreRetriedMessage);

					using (var stream = bus.Configuration.Serializer.Serialize(transportMessage))
					{
						var handler = state.GetMessageHandler();
						var handlerFullTypeName = handler != null ? handler.GetType().FullName : "(handler is null)";
						var currentRetryCount = transportMessage.FailureMessages.Count;

						var retry = !(pipelineEvent.Pipeline.Exception is UnrecoverableHandlerException)
						            &&
						            action.Retry;

						if (retry)
						{
							_log.Warning(string.Format(ESBResources.MessageHandlerExceptionWillRetry,
							                           handlerFullTypeName,
							                           pipelineEvent.Pipeline.Exception.AllMessages(),
							                           transportMessage.MessageType,
							                           transportMessage.MessageId,
							                           currentRetryCount,
							                           state.GetMaximumFailureCount()));

							state.GetWorkQueue().Enqueue(transportMessage.MessageId, stream);
						}
						else
						{
							_log.Error(string.Format(ESBResources.MessageHandlerExceptionFailure,
							                         handlerFullTypeName,
							                         pipelineEvent.Pipeline.Exception.AllMessages(),
							                         transportMessage.MessageType,
							                         transportMessage.MessageId,
							                         state.GetMaximumFailureCount(),
							                         state.GetErrorQueue().Uri));

							state.GetErrorQueue().Enqueue(transportMessage.MessageId, stream);
						}
					}

					state.GetWorkQueue().Acknowledge(receivedMessage.AcknowledgementToken);
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