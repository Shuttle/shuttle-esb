namespace Shuttle.ESB.Core
{
	internal class ServiceBusEvents : IServiceBusEvents
	{
		public event BeforePipelineExceptionHandledDelegate BeforePipelineExceptionHandled = delegate { };
		public event AfterPipelineExceptionHandledDelegate AfterPipelineExceptionHandled = delegate { };
		public event TransportMessageDeserializationExceptionDelegate TransportMessageDeserializationException = delegate { };
		public event MessageDeserializationExceptionDelegate MessageDeserializationException = delegate { };
		public event QueueEmptyDelegate QueueEmpty = delegate { };
		public event MessageNotHandledDelegate MessageNotHandled = delegate { };
		public event HandlerExceptionDelegate HandlerException = delegate { };

        public event PipelineCreatedDelegate PipelineCreated = delegate { };
	    public event PipelineObtainedDelegate PipelineObtained = delegate { };
        public event PipelineReleaseDelegate PipelineReleased = delegate { };
        
        public event ThreadWorkingDelegate ThreadWorking = delegate { };
        public event ThreadWaitingDelegate ThreadWaiting = delegate { };

		public void OnBeforePipelineExceptionHandled(object sender, PipelineExceptionEventArgs args)
		{
            BeforePipelineExceptionHandled.Invoke(sender, args);
		}

        public void OnAfterPipelineExceptionHandled(object sender, PipelineExceptionEventArgs args)
		{
            AfterPipelineExceptionHandled.Invoke(sender, args);
		}

		public void OnTransportMessageDeserializationException(object sender, DeserializationExceptionEventArgs args)
		{
			TransportMessageDeserializationException.Invoke(sender, args);
		}

		public void OnMessageDeserializationException(object sender, DeserializationExceptionEventArgs args)
		{
			TransportMessageDeserializationException.Invoke(sender, args);
		}

		public void OnQueueEmpty(object sender, QueueEmptyEventArgs args)
		{
			QueueEmpty.Invoke(sender, args);
		}

		public void OnMessageNotHandled(object sender, MessageNotHandledEventArgs args)
		{
			MessageNotHandled.Invoke(sender, args);
		}

		public void OnHandlerException(object sender, HandlerExceptionEventArgs args)
		{
			HandlerException.Invoke(sender, args);
		}

		public void OnPipelineCreated(object sender, PipelineEventArgs args)
		{
            PipelineCreated.Invoke(sender, args);
		}

	    public void OnPipelineObtained(object sender, PipelineEventArgs args)
		{
            PipelineObtained.Invoke(sender, args);
		}

		public void OnPipelineReleased(object sender, PipelineEventArgs args)
		{
            PipelineReleased.Invoke(sender, args);
		}

	    public void OnThreadWorking(object sender, ThreadStateEventArgs args)
	    {
	        ThreadWorking.Invoke(sender, args);
	    }

	    public void OnThreadWaiting(object sender, ThreadStateEventArgs args)
	    {
	        ThreadWaiting(sender, args);
	    }
	}
}