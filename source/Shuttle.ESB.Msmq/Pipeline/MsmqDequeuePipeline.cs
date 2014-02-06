using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Msmq
{
	public class MsmqDequeuePipeline : ObservablePipeline
	{
		public MsmqDequeuePipeline()
		{
			RegisterStage("Dequeue")
				.WithEvent<OnStart>()
				.WithEvent<OnBeginTransaction>()
				.WithEvent<OnReceiveMessage>()
				.WithEvent<OnSendJournalMessage>()
				.WithEvent<OnCommitTransaction>()
				.WithEvent<OnDispose>();

			RegisterObserver(new MsmqDequeueObserver());
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