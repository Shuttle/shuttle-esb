using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SendMessagePipeline : MessagePipeline
	{
		public SendMessagePipeline(IServiceBus bus)
			: base(bus)
		{
			OnExecuteRaiseEvent<OnPrepareMessage>()
				.ThenEvent<OnFindRouteForMessage>()
				.ThenEvent<OnSerializeMessage>()
				.ThenEvent<OnCompressMessage>()
				.ThenEvent<OnEncryptMessage>()
				.ThenEvent<OnSerializeTransportMessage>()
				.ThenEvent<OnSendMessage>();

			RegisterObserver(new PrepareMessageObserver())
				.AndObserver(new FindMessageRouteObserver())
				.AndObserver(new SerializeMessageObserver())
				.AndObserver(new SerializeTransportMessageObserver())
				.AndObserver(new CompressMessageObserver())
				.AndObserver(new EncryptMessageObserver())
				.AndObserver(new SendMessageObserver());
		}

		public bool Execute(IMessage message, IQueue queue)
		{
			Guard.AgainstNull(message, "message");

			Message = message;
			DestinationQueue = DestinationQueue ?? queue;

			return base.Execute();
		}
	}
}