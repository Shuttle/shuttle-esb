using System;

namespace Shuttle.ESB.Core
{
	internal class ServiceBusEvents : IServiceBusEvents
	{
		public event BeforeEnqueueStreamDelegate BeforeEnqueueStream = delegate { };
		public event BeforeDequeueStreamDelegate BeforeDequeueStream = delegate { };
		public event BeforeHandleMessageDelegate BeforeHandleMessage = delegate { };
		public event BeforeRemoveMessageDelegate BeforeRemoveMessage = delegate { };
		public event BeforeDistributeMessageDelegate BeforeDistributeMessage = delegate { };
		public event AfterEnqueueStreamDelegate AfterEnqueueStream = delegate { };
		public event AfterDequeueStreamDelegate AfterDequeueStream = delegate { };
		public event AfterHandleMessageDelegate AfterHandleMessage = delegate { };
		public event AfterRemoveMessageDelegate AfterRemoveMessage = delegate { };
		public event AfterDistributeMessageDelegate AfterDistributeMessage = delegate { };
		public event AfterMessageSerializationDelegate AfterMessageSerialization = delegate { };
		public event AfterMessageDeserializationDelegate AfterMessageDeserialization = delegate { };
        public event AfterTransportMessageSerializationDelegate AfterTransportMessageSerialization = delegate { };
        public event AfterTransportMessageDeserializationDelegate AfterTransportMessageDeserialization = delegate { };
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

		public void OnBeforeEnqueueStream(object sender, QueueMessageEventArgs args)
		{
			BeforeEnqueueStream.Invoke(sender, args);
		}

		public void OnBeforeDequeueMessage(object sender, BeforeDequeueEventArgs args)
		{
			BeforeDequeueStream.Invoke(sender, args);
		}

		public void OnBeforeHandleMessage(object sender, BeforeHandleMessageEventArgs args)
		{
			BeforeHandleMessage.Invoke(sender, args);
		}

		public void OnBeforeRemoveMessage(object sender, QueueMessageEventArgs args)
		{
			BeforeRemoveMessage.Invoke(sender, args);
		}

		public void OnBeforeDistributeMessage(object sender, DistributeMessageEventArgs args)
		{
			BeforeDistributeMessage.Invoke(sender, args);
		}

		public void OnAfterEnqueueStream(object sender, QueueMessageEventArgs args)
		{
			AfterEnqueueStream.Invoke(sender, args);
		}

		public void OnAfterDequeueStream(object sender, QueueStreamEventArgs args)
		{
			AfterDequeueStream.Invoke(sender, args);
		}

		public void OnAfterHandleMessage(object sender, AfterHandleMessageEventArgs args)
		{
			AfterHandleMessage.Invoke(sender, args);
		}

		public void OnAfterRemoveMessage(object sender, QueueMessageEventArgs args)
		{
			AfterRemoveMessage.Invoke(sender, args);
		}

		public void OnAfterDistributeMessage(object sender, DistributeMessageEventArgs args)
		{
			AfterDistributeMessage.Invoke(sender, args);
		}

		public void OnAfterMessageSerialization(object sender, MessageSerializationEventArgs args)
		{
			AfterMessageSerialization.Invoke(sender, args);
		}

		public void OnAfterMessageDeserialization(object sender, MessageSerializationEventArgs args)
		{
			AfterMessageDeserialization.Invoke(sender, args);
		}

        public void OnAfterTransportMessageSerialization(object sender, TransportMessageSerializationEventArgs args)
		{
            AfterTransportMessageSerialization.Invoke(sender, args);
		}

        public void OnAfterTransportMessageDeserialization(object sender, TransportMessageSerializationEventArgs args)
		{
            AfterTransportMessageDeserialization.Invoke(sender, args);
		}

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