using System;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqReturnJournalPipeline : Pipeline
	{
		public MsmqReturnJournalPipeline()
		{
			RegisterStage("Return")
				.WithEvent<OnStart>()
				.WithEvent<OnBeginTransaction>()
				.WithEvent<OnReturnJournalMessages>()
				.WithEvent<OnCommitTransaction>()
				.WithEvent<OnDispose>();

			RegisterObserver(new MsmqTransactionObserver());
			RegisterObserver(new MsmqReturnJournalObserver());
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