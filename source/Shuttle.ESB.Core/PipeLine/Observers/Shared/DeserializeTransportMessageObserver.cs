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
			var state = pipelineEvent.Pipeline.State;
			Guard.AgainstNull(state.GetTransportMessageStream(), "transportMessageStream");
			Guard.AgainstNull(state.GetWorkQueue(), "workQueue");
			Guard.AgainstNull(state.GetErrorQueue(), "errorQueue");

			TransportMessage transportMessage;

			try
			{
				using (var stream = state.GetTransportMessageStream().Copy())
				{
					transportMessage = (TransportMessage)state.GetServiceBus().Configuration.Serializer.Deserialize(typeof(TransportMessage), stream);
				}
			}
			catch (Exception ex)
			{
                _log.Error(ex.ToString());
                _log.Error(string.Format(ESBResources.TransportMessageDeserializationException, state.GetWorkQueue().Uri, ex));

				state.SetTransactionComplete();
				pipelineEvent.Pipeline.Abort();

				state.GetServiceBus().Events.OnTransportMessageDeserializationException(this,
					new DeserializationExceptionEventArgs(
						pipelineEvent, 
						state.GetWorkQueue(),
						state.GetErrorQueue(),
						ex));

				return;
			}

			state.SetTransportMessage(transportMessage);
			state.SetMessageBytes(transportMessage.Message);

			transportMessage.AcceptInvariants();

			state.GetServiceBus().Events.OnAfterTransportMessageDeserialization(
					this,
					new TransportMessageSerializationEventArgs(pipelineEvent, transportMessage));

            if (_log.IsVerboseEnabled)
            {
                _log.Verbose(string.Format(ESBResources.TransportMessageDeserialized, transportMessage.MessageType,
                                          transportMessage.MessageId));
            }
		}
	}
}