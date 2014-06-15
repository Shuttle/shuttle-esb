using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class OnStart : PipelineEvent
	{
	}

	public class OnBeginTransaction : PipelineEvent
	{
	}

	public class OnReceiveMessage : PipelineEvent
	{
	}

	public class OnSendJournalMessage : PipelineEvent
	{
	}

	public class OnReturnJournalMessages : PipelineEvent
	{
	}

	public class OnReleaseMessage : PipelineEvent
	{
	}

	public class OnCommitTransaction : PipelineEvent
	{
	}

	public class OnDispose : PipelineEvent
	{
	}
}