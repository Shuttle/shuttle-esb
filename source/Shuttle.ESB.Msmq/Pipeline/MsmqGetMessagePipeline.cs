using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Msmq
{
	public class MsmqGetMessagePipeline : ObservablePipeline
	{
		public MsmqGetMessagePipeline()
		{
			RegisterStage("Dequeue")
				.WithEvent<OnStart>()
				.WithEvent<OnBeginTransaction>()
				.WithEvent<OnReceiveMessage>()
				.WithEvent<OnSendJournalMessage>()
				.WithEvent<OnCommitTransaction>()
				.WithEvent<OnDispose>();

			RegisterObserver(new MsmqTransactionObserver());
			RegisterObserver(new MsmqGetMessageObserver());
		}

		public bool Execute(MsmqUriParser parser, Guid messageId, TimeSpan timeout)
		{
			State.Clear();

			State.Add(parser);
			State.Add("messageId", messageId);
			State.Add("timeout", timeout);

			return base.Execute();
		}
	}
}