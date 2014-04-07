namespace Shuttle.ESB.Core
{
	public delegate void BeforeDistributeMessageDelegate(object sender, DistributeMessageEventArgs e);

	public delegate void AfterEnqueueStreamDelegate(object sender, QueueMessageEventArgs e);

	public delegate void AfterDequeueStreamDelegate(object sender, QueueStreamEventArgs e);

	public delegate void AfterHandleMessageDelegate(object sender, AfterHandleMessageEventArgs e);

	public delegate void AfterRemoveMessageDelegate(object sender, QueueMessageEventArgs e);

	public delegate void AfterDistributeMessageDelegate(object sender, DistributeMessageEventArgs e);

	public delegate void AfterPrepareMessageDelegate(object sender, QueueMessageEventArgs e);

	public delegate void AfterMessageSerializationDelegate(object sender, MessageSerializationEventArgs e);

	public delegate void AfterMessageDeserializationDelegate(object sender, MessageSerializationEventArgs e);

	public delegate void AfterTransportMessageSerializationDelegate(object sender, TransportMessageSerializationEventArgs e
		);

	public delegate void AfterTransportMessageDeserializationDelegate(
		object sender, TransportMessageSerializationEventArgs e);

	public delegate void BeforePipelineExceptionHandledDelegate(object sender, PipelineExceptionEventArgs e);

	public delegate void AfterPipelineExceptionHandledDelegate(object sender, PipelineExceptionEventArgs e);

	public delegate void TransportMessageDeserializationExceptionDelegate(
		object sender, DeserializationExceptionEventArgs e);

	public delegate void MessageDeserializationExceptionDelegate(object sender, DeserializationExceptionEventArgs e);

	public delegate void QueueEmptyDelegate(object sender, QueueEmptyEventArgs e);

	public delegate void MessageNotHandledDelegate(object sender, MessageNotHandledEventArgs e);

	public delegate void HandlerExceptionDelegate(object sender, HandlerExceptionEventArgs e);

	public delegate void PipelineCreatedDelegate(object sender, PipelineEventArgs e);

    public delegate void PipelineObtainedDelegate(object sender, PipelineEventArgs e);

	public delegate void PipelineReleaseDelegate(object sender, PipelineEventArgs e);
	
    public delegate void ThreadWorkingDelegate(object sender, ThreadStateEventArgs e);
    public delegate void ThreadWaitingDelegate(object sender, ThreadStateEventArgs e);

	public interface IServiceBusEvents
	{
		event BeforeDistributeMessageDelegate BeforeDistributeMessage;
		event AfterEnqueueStreamDelegate AfterEnqueueStream;
		event AfterDequeueStreamDelegate AfterDequeueStream;
		event AfterHandleMessageDelegate AfterHandleMessage;
		event AfterRemoveMessageDelegate AfterRemoveMessage;
		event AfterDistributeMessageDelegate AfterDistributeMessage;
		event AfterMessageSerializationDelegate AfterMessageSerialization;
		event AfterMessageDeserializationDelegate AfterMessageDeserialization;
		event AfterTransportMessageSerializationDelegate AfterTransportMessageSerialization;
		event AfterTransportMessageDeserializationDelegate AfterTransportMessageDeserialization;
		event BeforePipelineExceptionHandledDelegate BeforePipelineExceptionHandled;
		event AfterPipelineExceptionHandledDelegate AfterPipelineExceptionHandled;
		event TransportMessageDeserializationExceptionDelegate TransportMessageDeserializationException;
		event MessageDeserializationExceptionDelegate MessageDeserializationException;

		event QueueEmptyDelegate QueueEmpty;
		event MessageNotHandledDelegate MessageNotHandled;
		event HandlerExceptionDelegate HandlerException;

		event PipelineCreatedDelegate PipelineCreated;
	    event PipelineObtainedDelegate PipelineObtained;
		event PipelineReleaseDelegate PipelineReleased;
		
        event ThreadWorkingDelegate ThreadWorking;
        event ThreadWaitingDelegate ThreadWaiting;

		void OnBeforeDistributeMessage(object sender, DistributeMessageEventArgs args);
		void OnAfterEnqueueStream(object sender, QueueMessageEventArgs args);
		void OnAfterDequeueStream(object sender, QueueStreamEventArgs args);
		void OnAfterHandleMessage(object sender, AfterHandleMessageEventArgs args);
		void OnAfterRemoveMessage(object sender, QueueMessageEventArgs args);
		void OnAfterDistributeMessage(object sender, DistributeMessageEventArgs args);
		void OnAfterMessageSerialization(object sender, MessageSerializationEventArgs args);
		void OnAfterMessageDeserialization(object sender, MessageSerializationEventArgs args);
		void OnAfterTransportMessageSerialization(object sender, TransportMessageSerializationEventArgs args);
		void OnAfterTransportMessageDeserialization(object sender, TransportMessageSerializationEventArgs args);
		void OnBeforePipelineExceptionHandled(object sender, PipelineExceptionEventArgs args);
		void OnAfterPipelineExceptionHandled(object sender, PipelineExceptionEventArgs args);
		void OnTransportMessageDeserializationException(object sender, DeserializationExceptionEventArgs args);
		void OnMessageDeserializationException(object sender, DeserializationExceptionEventArgs args);
		void OnQueueEmpty(object sender, QueueEmptyEventArgs args);
		void OnMessageNotHandled(object sender, MessageNotHandledEventArgs args);
		void OnHandlerException(object sender, HandlerExceptionEventArgs args);

		void OnPipelineCreated(object sender, PipelineEventArgs args);
	    void OnPipelineObtained(object sender, PipelineEventArgs args);
		void OnPipelineReleased(object sender, PipelineEventArgs args);
		
        void OnThreadWorking(object sender, ThreadStateEventArgs args);
        void OnThreadWaiting(object sender, ThreadStateEventArgs args);
	}
}