using System;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqReleaseMessagePipeline : Pipeline
	{
		public MsmqReleaseMessagePipeline()
		{
			RegisterStage("Release")
				.WithEvent<OnStart>()
				.WithEvent<OnBeginTransaction>()
				.WithEvent<OnReleaseMessage>()
				.WithEvent<OnCommitTransaction>()
				.WithEvent<OnDispose>();

			RegisterObserver(new MsmqTransactionObserver());
			RegisterObserver(new MsmqReleaseMessageObserver());
		}

		public bool Execute(Guid messageId, MsmqUriParser parser, TimeSpan timeout)
		{
			State.Clear();

			State.Add("messageId", messageId);
			State.Add(parser);
			State.Add("timeout", timeout);

			return base.Execute();
		}
	}
}