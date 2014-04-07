using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SendOutboxMessageObserver : IPipelineObserver<OnSendMessage>
	{
		private readonly ILog _log;

		public SendOutboxMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnSendMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var transportMessage = state.GetTransportMessage();

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNullOrEmptyString(transportMessage.RecipientInboxWorkQueueUri, "uri");

			var queue =
				state.GetServiceBus().Configuration.QueueManager.GetQueue(transportMessage.RecipientInboxWorkQueueUri);

			if (_log.IsVerboseEnabled)
			{
				_log.Verbose(string.Format(ESBResources.EnqueueMessage,
				                          transportMessage.MessageType,
				                          transportMessage.MessageId,
				                          queue.Uri));
			}

			using (var stream = state.GetTransportMessageStream().Copy())
			{
				queue.Enqueue(transportMessage.MessageId, stream);
			}

			state.GetServiceBus().Events.OnAfterEnqueueStream(
				this,
				new QueueMessageEventArgs(pipelineEvent, queue, transportMessage));
		}
	}
}