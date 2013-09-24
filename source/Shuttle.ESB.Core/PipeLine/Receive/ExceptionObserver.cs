using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ExceptionObserver :
		IPipelineObserver<OnPipelineException>
	{
		public void Execute(OnPipelineException pipelineEvent)
		{
			if (pipelineEvent.ExceptionHandled)
			{
				return;
			}

			if (pipelineEvent.Exception is MessageDeserializationException)
			{
				pipelineEvent.GetErrorQueue().Enqueue(pipelineEvent.GetWorkQueue().UnderlyingMessageData);
			}
			else
			{
				var transportMessage = pipelineEvent.GetTransportMessage();

				if (transportMessage == null)
				{
					return;
				}

				transportMessage.RegisterFailure(pipelineEvent.Exception.Message, pipelineEvent.GetDurationToIgnoreOnFailure());

				transportMessage.Message = pipelineEvent.GetMessageBytes();

				if (transportMessage.FailureMessages.Count < pipelineEvent.GetMaximumFailureCount())
				{
					pipelineEvent.GetWorkQueue().Enqueue(transportMessage.MessageId, pipelineEvent.GetServiceBus().Configuration.MessageSerializer.Serialize(transportMessage));
				}
				else
				{
					pipelineEvent.GetErrorQueue().Enqueue(transportMessage.MessageId, pipelineEvent.GetServiceBus().Configuration.MessageSerializer.Serialize(transportMessage));
				}

				var journal = pipelineEvent.GetJournalQueue();

				if (journal != null)
				{
					journal.Remove(transportMessage.MessageId);
				}
			}

			pipelineEvent.ExceptionHandled = true;
		}
	}
}