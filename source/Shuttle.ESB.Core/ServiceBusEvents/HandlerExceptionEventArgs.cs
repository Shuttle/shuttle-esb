using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class HandlerExceptionEventArgs : PipelineEventEventArgs
	{
		public IMessageHandler MessageHandler { get; private set; }
		public TransportMessage TransportMessage { get; private set; }
		public object Message { get; private set; }
		public IQueue WorkQueue { get; private set; }
		public IQueue ErrorQueue { get; private set; }
		public Exception Exception { get; private set; }

		public HandlerExceptionEventArgs(PipelineEvent pipelineEvent, IMessageHandler messageHandler,
		                                 TransportMessage transportMessage, object message, IQueue workQueue,
		                                 IQueue errorQueue, Exception exception)
			: base(pipelineEvent)
		{
			MessageHandler = messageHandler;
			TransportMessage = transportMessage;
			Message = message;
			WorkQueue = workQueue;
			ErrorQueue = errorQueue;
			Exception = exception;
		}
	}
}