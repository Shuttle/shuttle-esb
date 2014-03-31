using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Msmq
{
	public class MsmqReleaseMessagePipeline : ObservablePipeline
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
			if (!parser.Journal)
			{
				return false;
			}

			State.Clear();

			State.Add("messageId", messageId);
			State.Add(parser);
			State.Add("timeout", timeout);

			return base.Execute();
		}
	}
}