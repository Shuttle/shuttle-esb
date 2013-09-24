using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeserializeTransportMessageObserver : IPipelineObserver<OnDeserializeTransportMessage>
	{
		private readonly ILog log;

		public DeserializeTransportMessageObserver()
		{
			log = Log.For(this);
		}

		public void Execute(OnDeserializeTransportMessage pipelineEvent)
		{
			Guard.AgainstNull(pipelineEvent.GetTransportMessageStream(), "dequeuedStream");
			Guard.AgainstNull(pipelineEvent.GetWorkQueue(), "workQueue");
			Guard.AgainstNull(pipelineEvent.GetErrorQueue(), "errorQueue");

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
                Log.Error(ex.ToString());
                log.Error(string.Format(ESBResources.TransportMessageDeserializationException, pipelineEvent.GetWorkQueue().Uri, ex));

				pipelineEvent.GetErrorQueue().Enqueue(pipelineEvent.GetWorkQueue().UnderlyingMessageData);
				pipelineEvent.SetTransactionComplete();
				pipelineEvent.Pipeline.Abort();

				pipelineEvent.GetServiceBus().Events.OnTransportMessageDeserializationException(this,
					new DeserializationExceptionEventArgs(
						pipelineEvent, 
						pipelineEvent.GetWorkQueue(),
						pipelineEvent.GetErrorQueue(),
						ex));

				return;
			}

			pipelineEvent.SetTransportMessage(transportMessage);
			pipelineEvent.SetMessageBytes(transportMessage.Message);

			transportMessage.AcceptInvariants();

			pipelineEvent.GetServiceBus().Events.OnAfterTransportMessageDeserialization(
					this,
					new TransportMessageSerializationEventArgs(pipelineEvent, transportMessage));

            if (log.IsVerboseEnabled)
            {
                log.Verbose(string.Format(ESBResources.TransportMessageDeserialized, transportMessage.MessageType,
                                          transportMessage.MessageId));
            }

		    if (!transportMessage.Ignore())
			{
				pipelineEvent.SetWorking();

				return;
			}

			using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
			{
				pipelineEvent.GetWorkQueue().Enqueue(transportMessage.MessageId, stream);
			}

			pipelineEvent.SetTransactionComplete();
			pipelineEvent.Pipeline.Abort();

            if (log.IsVerboseEnabled)
            {
                log.Verbose(string.Format(ESBResources.TransportMessageIgnored, transportMessage.MessageId,
                                          transportMessage.IgnoreTillDate.ToString(ESBResources.FormatDateFull)));
            }
		}
	}
}