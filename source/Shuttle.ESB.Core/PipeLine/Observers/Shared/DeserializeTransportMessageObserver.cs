using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeserializeTransportMessageObserver : IPipelineObserver<OnDeserializeTransportMessage>
	{
		private readonly ILog _log;

		public DeserializeTransportMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnDeserializeTransportMessage pipelineEvent)
		{
			Guard.AgainstNull(pipelineEvent.GetTransportMessageStream(), "transportMessageStream");
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
                _log.Error(ex.ToString());
                _log.Error(string.Format(ESBResources.TransportMessageDeserializationException, pipelineEvent.GetWorkQueue().Uri, ex));

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

            if (_log.IsVerboseEnabled)
            {
                _log.Verbose(string.Format(ESBResources.TransportMessageDeserialized, transportMessage.MessageType,
                                          transportMessage.MessageId));
            }

		    if (!transportMessage.IsIgnoring())
			{
				pipelineEvent.SetWorking();

				return;
			}

			using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
			{
				if (pipelineEvent.GetDeferredQueue() == null)
				{
					pipelineEvent.GetWorkQueue().Enqueue(transportMessage.MessageId, stream);
				}
				else
				{
					pipelineEvent.GetDeferredQueue().Enqueue(transportMessage.MessageId, stream);

					pipelineEvent.GetServiceBus().Configuration.Inbox.MessageDeferred(transportMessage.IgnoreTillDate);
				}
			}

			pipelineEvent.SetTransactionComplete();
			pipelineEvent.Pipeline.Abort();

            if (_log.IsVerboseEnabled)
            {
                _log.Verbose(string.Format(ESBResources.TransportMessageIgnored, transportMessage.MessageId,
                                          transportMessage.IgnoreTillDate.ToString(ESBResources.FormatDateFull)));
            }
		}
	}
}