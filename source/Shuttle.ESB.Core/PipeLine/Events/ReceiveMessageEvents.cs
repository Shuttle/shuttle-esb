using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class OnGetMessage : PipelineEvent
	{
	}

	public class OnAfterGetMessage : PipelineEvent
	{
	}

	public class OnDeserializeTransportMessage : PipelineEvent
	{
	}

	public class OnAfterDeserializeTransportMessage : PipelineEvent
	{
	}

	public class OnDecryptMessage : PipelineEvent
	{
	}

	public class OnDeserializeMessage : PipelineEvent
	{
	}

	public class OnHandleMessage : PipelineEvent
	{
	}

	public class OnAfterHandleMessage : PipelineEvent
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

	public class OnSendDeferred : PipelineEvent
	{
	}

	public class OnProcessDeferredMessage : PipelineEvent
	{
	}

	public class OnAfterProcessDeferredMessage : PipelineEvent
	{
	}
}