using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class OnPeek : PipelineEvent
	{
	}

	public class OnDequeue : PipelineEvent
	{
	}

	public class OnDeserializeTransportMessage : PipelineEvent
	{
	}

	public class OnDecryptMessage : PipelineEvent
	{
	}

	public class OnDeserializeMessage : PipelineEvent
	{
	}

	public class OnMessageReceived : PipelineEvent
	{
	}

	public class OnEnqueueJournal : PipelineEvent
	{
	}

	public class OnHandleMessage : PipelineEvent
	{
	}

	public class OnMessageHandled : PipelineEvent
	{
	}

	public class OnRemoveJournalMessage : PipelineEvent
	{
	}

	public class OnStartTransactionScope : PipelineEvent
	{
	}

	public class OnCompleteTransactionScope : PipelineEvent
	{
	}

	public class OnDisposeTransactionScope : PipelineEvent
	{
	}

	public class OnAcknowledgeMessage : PipelineEvent
	{
	}

	public class OnCompressMessage : PipelineEvent
	{
	}
}