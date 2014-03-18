using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SendMessagePipeline : MessagePipeline
	{
		public SendMessagePipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Send")
				.WithEvent<OnPrepareMessage>()
				.WithEvent<OnFindRouteForMessage>()
				.WithEvent<OnSerializeMessage>()
                .WithEvent<OnEncryptMessage>()
                .WithEvent<OnCompressMessage>()
				.WithEvent<OnSerializeTransportMessage>()
				.WithEvent<OnSendMessage>();

			RegisterObserver(new PrepareMessageObserver());
			RegisterObserver(new FindMessageRouteObserver());
			RegisterObserver(new SerializeMessageObserver());
			RegisterObserver(new SerializeTransportMessageObserver());
			RegisterObserver(new CompressMessageObserver());
			RegisterObserver(new EncryptMessageObserver());
			RegisterObserver(new SendMessageObserver());
		}

		public bool Execute(DateTime ignoreTillDate, object message, IQueue queue)
		{
			Guard.AgainstNull(message, "message");

			SetIgnoreTillDate(ignoreTillDate);
			SetMessage(message);
			SetDestinationQueue(queue);

			return base.Execute();
		}
	}
}