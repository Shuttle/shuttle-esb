namespace Shuttle.ESB.Core
{
	public delegate void BeforePipelineExceptionHandledDelegate(object sender, PipelineExceptionEventArgs e);

	public delegate void AfterPipelineExceptionHandledDelegate(object sender, PipelineExceptionEventArgs e);

	public delegate void TransportMessageDeserializationExceptionDelegate(object sender, DeserializationExceptionEventArgs e);

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