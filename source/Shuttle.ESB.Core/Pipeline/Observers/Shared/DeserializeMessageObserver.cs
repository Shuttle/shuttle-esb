using System;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeserializeMessageObserver : IPipelineObserver<OnDeserializeMessage>
	{
		private readonly ILog log;

		public DeserializeMessageObserver()
		{
			log = Log.For(this);
		}

		public void Execute(OnDeserializeMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			Guard.AgainstNull(state.GetTransportMessage(), "transportMessage");
			Guard.AgainstNull(state.GetWorkQueue(), "workQueue");
			Guard.AgainstNull(state.GetErrorQueue(), "errorQueue");

			var transportMessage = state.GetTransportMessage();

			object message;

		    try
		    {
		        using (var stream = new MemoryStream(transportMessage.Message))
		        {
		            message = state.GetServiceBus().Configuration.Serializer.Deserialize(Type.GetType(transportMessage.AssemblyQualifiedName, true, true), stream);
		        }
		    }
            catch (Exception ex)
            {
				transportMessage.RegisterFailure(ex.AllMessages(), new TimeSpan());

				state.GetErrorQueue().Enqueue(transportMessage.MessageId, state.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
				
				state.SetTransactionComplete();
				pipelineEvent.Pipeline.Abort();

                state.GetServiceBus().Events.OnMessageDeserializationException(this,
                    new DeserializationExceptionEventArgs(
						pipelineEvent, 
                        state.GetWorkQueue(),
                        state.GetErrorQueue(),
                        ex));

                return;
            }
            
            state.SetMessage(message);

			if (log.IsVerboseEnabled)
            {
                log.Trace(string.Format(ESBResources.TraceMessageDeserialized, message.GetType(), transportMessage.MessageId));
            }
		}
	}
}