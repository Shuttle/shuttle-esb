using System;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeserializeMessageObserver : IPipelineObserver<OnDeserializeMessage>
	{
		public void Execute(OnDeserializeMessage pipelineEvent)
		{
			Guard.AgainstNull(pipelineEvent.GetTransportMessage(), "transportMessage");
			Guard.AgainstNull(pipelineEvent.GetWorkQueue(), "workQueue");
			Guard.AgainstNull(pipelineEvent.GetErrorQueue(), "errorQueue");

			var transportMessage = pipelineEvent.GetTransportMessage();

			IMessage message;
			
			using (var stream = new MemoryStream(transportMessage.Message))
			{
				message = (IMessage)pipelineEvent.GetServiceBus()
				                    	.Configuration.MessageSerializer.Deserialize(Type.GetType(transportMessage.AssemblyQualifiedName, true, true), stream);
			}

			pipelineEvent.SetMessage(message);
			
			pipelineEvent.GetServiceBus().Events.OnAfterDeserialization(
					this,
					new MessageSerializationEventArgs(transportMessage, message));

			Log.Debug(string.Format(Resources.DebugMessageDeserialized, message.GetType(), transportMessage.MessageId));
		}
	}
}