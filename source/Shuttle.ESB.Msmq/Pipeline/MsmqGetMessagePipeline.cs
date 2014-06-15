using System;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqGetMessagePipeline : Pipeline
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

		public bool Execute(MsmqUriParser parser, TimeSpan timeout)
		{
			State.Clear();

			State.Add(parser);
			State.Add("timeout", timeout);

			return base.Execute();
		}
	}
}