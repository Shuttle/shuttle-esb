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
			Guard.AgainstNull(pipelineEvent.GetTransportMessage(), "transportMessage");
			Guard.AgainstNull(pipelineEvent.GetWorkQueue(), "workQueue");
			Guard.AgainstNull(pipelineEvent.GetErrorQueue(), "errorQueue");

			var transportMessage = pipelineEvent.GetTransportMessage();

			object message;

		    try
		    {
		        using (var stream = new MemoryStream(transportMessage.Message))
		        {
		            message = pipelineEvent.GetServiceBus().Configuration.Serializer.Deserialize(Type.GetType(transportMessage.AssemblyQualifiedName, true, true), stream);
		        }
		    }
            catch (Exception ex)
            {
				transportMessage.RegisterFailure(ex.CompactMessages(), new TimeSpan());

				pipelineEvent.GetErrorQueue().Enqueue(transportMessage.MessageId, pipelineEvent.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
				
				pipelineEvent.SetTransactionComplete();
				pipelineEvent.Pipeline.Abort();

                pipelineEvent.GetServiceBus().Events.OnMessageDeserializationException(this,
                    new DeserializationExceptionEventArgs(
						pipelineEvent, 
                        pipelineEvent.GetWorkQueue(),
                        pipelineEvent.GetErrorQueue(),
                        ex));

                return;
            }
            
            pipelineEvent.SetMessage(message);
			
			pipelineEvent.GetServiceBus().Events.OnAfterMessageDeserialization(
					this,
					new MessageSerializationEventArgs(pipelineEvent, transportMessage, message));

            if (log.IsVerboseEnabled)
            {
                log.Verbose(string.Format(ESBResources.MessageDeserialized, message.GetType(), transportMessage.MessageId));
            }
		}
	}
}