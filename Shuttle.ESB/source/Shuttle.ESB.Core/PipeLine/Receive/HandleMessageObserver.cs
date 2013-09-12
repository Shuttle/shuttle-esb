using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class HandleMessageObserver :
		IPipelineObserver<OnHandleMessage>,
		IPipelineObserver<OnMessageReceived>
	{
		private static void InvokeHandler(IServiceBus bus, IMessageHandler handler, TransportMessage transportMessage, IMessage message)
		{
			var contextType = typeof(HandlerContext<>).MakeGenericType(new[] { message.GetType() });
			var method = handler.GetType().GetMethod("ProcessMessage", new[] { contextType });

			Guard.Against<ProcessMessageMethodMissingException>(method == null,
																string.Format(
																	Resources.ProcessMessageMethodMissingException,
																	handler.GetType().FullName,
																	transportMessage.Message.GetType().FullName));

			Log.Debug(string.Format(Resources.DebugMessageHandlerInvoke,
									transportMessage.Message.GetType().FullName,
									transportMessage.MessageId,
									handler.GetType().FullName));

			method.Invoke(handler, new[]
			                       	{
			                       		Activator.CreateInstance(contextType,
			                       		                         new object[] {bus, transportMessage, message})
			                       	});
		}

		public void Execute(OnHandleMessage pipelineEvent)
		{
			var bus = pipelineEvent.GetServiceBus();

			var transportMessage = pipelineEvent.GetTransportMessage();
			var message = pipelineEvent.GetMessage();

			var handler = bus.Configuration.MessageHandlerFactory.GetHandler(message);

			if (handler == null)
			{
				using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
				{
					pipelineEvent.GetErrorQueue().Enqueue(transportMessage.MessageId, stream);
				}

				return;
			}

			bus.Events.OnBeforeHandleMessage(this, new BeforeHandleMessageEventArgs(transportMessage));

			InvokeHandler(bus, handler, transportMessage, message);

			bus.Events.OnAfterHandleMessage(this,
											new AfterHandleMessageEventArgs(
												pipelineEvent.GetWorkQueue(),
												transportMessage));

			bus.Configuration.MessageHandlerFactory.ReleaseHandler(handler);
		}

		public void Execute(OnMessageReceived pipelineEvent)
		{
			pipelineEvent.GetServiceBus().TransportMessageReceived = pipelineEvent.GetTransportMessage();
		}
	}
}