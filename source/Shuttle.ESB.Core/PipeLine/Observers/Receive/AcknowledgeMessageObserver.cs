using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class AcknowledgeMessageObserver :
		IPipelineObserver<OnAcknowledgeMessage>
	{
		private readonly ILog _log;

		public AcknowledgeMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnAcknowledgeMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			if (pipelineEvent.Pipeline.Exception != null && !state.GetTransactionComplete())
			{
				return;
			}

			var bus = state.GetServiceBus();
			var transportMessage = state.GetTransportMessage();

			if (bus.Configuration.HasIdempotenceService)
			{
				try
				{
					bus.Configuration.IdempotenceService.ProcessingCompleted(transportMessage);
				}
				catch (Exception ex)
				{
					bus.Configuration.IdempotenceService.AccessException(_log, ex, pipelineEvent.Pipeline);
				}
			}

			state.GetWorkQueue().Acknowledge(transportMessage.MessageId);

			_log.Trace(string.Format(ESBResources.TraceAcknowledge, transportMessage.MessageType, transportMessage.MessageId));
		}
	}
}
