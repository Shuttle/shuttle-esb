using System.Messaging;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Msmq
{
	public class MsmqTransactionObserver :
		IPipelineObserver<OnStart>,
		IPipelineObserver<OnBeginTransaction>,
		IPipelineObserver<OnCommitTransaction>,
		IPipelineObserver<OnDispose>
	{
		public void Execute(OnStart pipelineEvent)
		{
			var parser = pipelineEvent.Pipeline.State.Get<MsmqUriParser>();

			if (!parser.Transactional)
			{
				return;
			}

			pipelineEvent.Pipeline.State.Add(new MessageQueueTransaction());
		}

		public void Execute(OnBeginTransaction pipelineEvent)
		{
			var tx = pipelineEvent.Pipeline.State.Get<MessageQueueTransaction>();

			if (tx == null)
			{
				return;
			}

			tx.Begin();
		}

		public void Execute(OnCommitTransaction pipelineEvent)
		{
			var tx = pipelineEvent.Pipeline.State.Get<MessageQueueTransaction>();

			if (tx == null)
			{
				return;
			}

			tx.Commit();
		}

		public void Execute(OnDispose pipelineEvent)
		{
			var tx = pipelineEvent.Pipeline.State.Get<MessageQueueTransaction>();

			if (tx == null)
			{
				return;
			}

			tx.Dispose();
			pipelineEvent.Pipeline.State.Replace<MessageQueueTransaction>(null);
		}
	}
}