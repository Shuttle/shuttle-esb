using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Modules.PurgeInbox
{
	public class PurgeInboxObserver : IPipelineObserver<OnAfterInitializeQueueFactories>
	{
		private readonly ILog _log;

		public PurgeInboxObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnAfterInitializeQueueFactories pipelineEvent)
		{
			var purge = pipelineEvent.Pipeline.State.GetServiceBus().Configuration.Inbox.WorkQueue as IPurgeQueue;

			if (purge == null)
			{
				_log.Warning(string.Format(ESBModuleResources.IPurgeQueueNotImplemented, pipelineEvent.Pipeline.State.GetServiceBus().Configuration.Inbox.WorkQueue.GetType().FullName));

				return;
			}

			purge.Purge();
		}
	}
}