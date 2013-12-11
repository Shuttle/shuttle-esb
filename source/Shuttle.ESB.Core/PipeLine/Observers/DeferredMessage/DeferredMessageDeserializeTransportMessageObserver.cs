using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeferredMessageDeserializeTransportMessageObserver : IPipelineObserver<OnDeserializeTransportMessage>
	{
		private readonly ILog _log;

		public DeferredMessageDeserializeTransportMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnDeserializeTransportMessage pipelineEvent)
		{
			Guard.AgainstNull(pipelineEvent.GetTransportMessageStream(), "transportMessageStream");

			TransportMessage transportMessage;

			try
			{
				using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
				{
					transportMessage = (TransportMessage)pipelineEvent.GetServiceBus().Configuration.Serializer.Deserialize(typeof(TransportMessage), stream);
				}
			}
			catch (Exception ex)
			{
                _log.Error(ex.ToString());
                _log.Error(string.Format(ESBResources.TransportMessageDeserializationException, "DeferredMessageQueue", ex));

				//TODO: error handling

				pipelineEvent.SetTransactionComplete();
				pipelineEvent.Pipeline.Abort();

				return;
			}

			pipelineEvent.SetTransportMessage(transportMessage);

			transportMessage.AcceptInvariants();

			pipelineEvent.GetServiceBus().Events.OnAfterTransportMessageDeserialization(
					this,
					new TransportMessageSerializationEventArgs(pipelineEvent, transportMessage));

            if (_log.IsVerboseEnabled)
            {
                _log.Verbose(string.Format(ESBResources.TransportMessageDeserialized, transportMessage.MessageType, transportMessage.MessageId));
            }
		}
	}
}