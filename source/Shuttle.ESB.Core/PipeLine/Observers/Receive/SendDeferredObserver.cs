using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class SendDeferredObserver :
		IPipelineObserver<OnSendDeferred>
    {
        private readonly ILog _log;

        public SendDeferredObserver()
        {
            _log = Log.For(this);
        }

		public void Execute(OnSendDeferred pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var configuration = state.GetServiceBus().Configuration;

			if (!configuration.HasIdempotenceService)
			{
				return;
			}

			var transportMessage = state.GetTransportMessage();
			var idempotenceService = configuration.IdempotenceService;

			try
			{
				foreach (var stream in idempotenceService.GetDeferredMessages(transportMessage))
				{
					var deferredTransportMessage = (TransportMessage)configuration.Serializer.Deserialize(typeof (TransportMessage), stream);

					state.GetServiceBus().Send(deferredTransportMessage);
				
					idempotenceService.DeferredMessageSent(transportMessage, deferredTransportMessage);
				}
			}
			catch (Exception ex)
			{
				idempotenceService.AccessException(_log, ex, pipelineEvent.Pipeline);
			}
		}
    }
}
