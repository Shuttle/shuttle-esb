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
			if (pipelineEvent.Pipeline.Exception != null && !pipelineEvent.GetTransactionComplete())
			{
				return;
			}

			var bus = pipelineEvent.GetServiceBus();
			var transportMessage = pipelineEvent.GetTransportMessage();

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

			var acknowledge = pipelineEvent.GetWorkQueue() as IAcknowledge;

			if (acknowledge == null)
			{
				return;
			}

			acknowledge.Acknowledge(transportMessage.MessageId);

			_log.Trace(string.Format(ESBResources.TraceAcknowledge, transportMessage.MessageType, transportMessage.MessageId));
		}
	}
}
