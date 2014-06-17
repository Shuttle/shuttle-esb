using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class TransportMessagePipeline : MessagePipeline
	{
		public TransportMessagePipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Create")
				.WithEvent<OnAssembleMessage>()
				.WithEvent<OnAfterPrepareMessage>()
				.WithEvent<OnSerializeMessage>()
				.WithEvent<OnAfterSerializeMessage>()
				.WithEvent<OnEncryptMessage>()
				.WithEvent<OnAfterEncryptMessage>()
				.WithEvent<OnCompressMessage>()
				.WithEvent<OnAfterCompressMessage>();

			RegisterObserver(new AssembleMessageObserver());
			RegisterObserver(new SerializeMessageObserver());
			RegisterObserver(new CompressMessageObserver());
			RegisterObserver(new EncryptMessageObserver());
		}

		public bool Execute(TransportMessageConfigurator configurator)
		{
			Guard.AgainstNull(configurator, "options");

			State.SetTransportMessageContext(configurator);

			return base.Execute();
		}

		public override bool Execute()
		{
			throw new NotImplementedException();
		}
	}
}