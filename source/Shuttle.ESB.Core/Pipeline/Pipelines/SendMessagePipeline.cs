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
				.WithEvent<OnAfterPrepareMessage>()
				.WithEvent<OnFindRouteForMessage>()
				.WithEvent<OnAfterFindRouteForMessage>()
				.WithEvent<OnSerializeMessage>()
				.WithEvent<OnAfterSerializeMessage>()
                .WithEvent<OnEncryptMessage>()
				.WithEvent<OnAfterEncryptMessage>()
                .WithEvent<OnCompressMessage>()
				.WithEvent<OnAfterCompressMessage>()
				.WithEvent<OnSerializeTransportMessage>()
				.WithEvent<OnAfterSerializeTransportMessage>()
				.WithEvent<OnSendMessage>()
				.WithEvent<OnAfterSendMessage>();

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

			State.SetIgnoreTillDate(ignoreTillDate);
			State.SetMessage(message);
			State.SetDestinationQueue(queue);

			return base.Execute();
		}
	}
}