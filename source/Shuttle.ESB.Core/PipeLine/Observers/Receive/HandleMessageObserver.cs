using System;
using System.Reflection;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class HandleMessageObserver :
        IPipelineObserver<OnHandleMessage>
    {
        private readonly ILog _log;

        public HandleMessageObserver()
        {
            _log = Log.For(this);
        }

        private void InvokeHandler(IServiceBus bus, IMessageHandler handler, TransportMessage transportMessage,
                                   object message, PipelineEvent pipelineEvent, Type messageType)
        {
            var contextType = typeof (HandlerContext<>).MakeGenericType(new[] {messageType});
            var method = handler.GetType().GetMethod("ProcessMessage", new[] {contextType});

            Guard.Against<ProcessMessageMethodMissingException>(method == null,
                                                                string.Format(
                                                                    ESBResources.ProcessMessageMethodMissingException,
                                                                    handler.GetType().FullName,
                                                                    transportMessage.MessageType));

            if (_log.IsTraceEnabled)
            {
                _log.Trace(string.Format(ESBResources.TraceCorrelationIdReceived, transportMessage.CorrelationId));

                foreach (var header in transportMessage.Headers)
                {
                    _log.Trace(string.Format(ESBResources.TraceTransportHeaderReceived, header.Key, header.Value));
                }

                _log.Trace(string.Format(ESBResources.MessageHandlerInvoke,
                                        transportMessage.MessageType,
                                        transportMessage.MessageId,
                                        handler.GetType().FullName));
            }

            try
            {
                method.Invoke(handler, new[]
                    {
                        Activator.CreateInstance(contextType,
                                                 new[]
                                                     {
                                                         bus,
                                                         transportMessage,
                                                         message,
                                                         pipelineEvent.GetActiveState()
                                                     })
                    });
            }
            catch (Exception ex)
            {
                var exception = ex.TrimLeading<TargetInvocationException>();

                bus.Events.OnHandlerException(
                    this,
                    new HandlerExceptionEventArgs(
                        pipelineEvent,
                        handler,
                        transportMessage,
                        message,
                        pipelineEvent.GetWorkQueue(),
                        pipelineEvent.GetErrorQueue(),
                        exception));

                throw exception;
            }
        }

        public void Execute(OnHandleMessage pipelineEvent)
        {
            var bus = pipelineEvent.GetServiceBus();
            var transportMessage = pipelineEvent.GetTransportMessage();

	        if (bus.Configuration.HasIdempotenceService)
	        {
		        try
		        {
			        if (!bus.Configuration.IdempotenceService.ShouldProcess(transportMessage))
			        {
						_log.Trace(string.Format(ESBResources.TraceMessageHandled, transportMessage.MessageType, transportMessage.MessageId));

				        pipelineEvent.Pipeline.Abort();

				        return;
			        }
		        }
		        catch (Exception ex)
		        {
			        bus.Configuration.IdempotenceService.AccessException(_log, ex, pipelineEvent.Pipeline);
		        }
	        }

	        try
	        {
		        pipelineEvent.GetServiceBus().HandlingTransportMessage(pipelineEvent.GetTransportMessage());
			
		        var message = pipelineEvent.GetMessage();

		        foreach (var uri in bus.Configuration.ForwardingRouteProvider.GetRouteUris(message))
		        {
			        if (_log.IsTraceEnabled)
			        {
				        _log.Trace(string.Format(ESBResources.TraceForwarding, transportMessage.MessageType,
				                                transportMessage.MessageId, new Uri(uri).Secured()));
			        }

			        bus.Send(message, uri);
		        }

		        var handler = bus.Configuration.MessageHandlerFactory.GetHandler(message);

		        pipelineEvent.SetMessageHandler(handler);

		        if (handler == null)
		        {
			        bus.Events.OnMessageNotHandled(this,
			                                       new MessageNotHandledEventArgs(
				                                       pipelineEvent,
				                                       pipelineEvent.GetWorkQueue(),
				                                       pipelineEvent.GetErrorQueue(),
				                                       transportMessage,
				                                       message));

			        if (!bus.Configuration.RemoveMessagesNotHandled)
			        {
				        var error = string.Format(ESBResources.MessageNotHandledFailure, message.GetType().FullName, transportMessage.MessageId, pipelineEvent.GetErrorQueue().Uri.Secured());

				        _log.Error(error);

						transportMessage.RegisterFailure(error);

				        using (var stream = bus.Configuration.Serializer.Serialize(transportMessage))
				        {
					        pipelineEvent.GetErrorQueue().Enqueue(transportMessage.MessageId, stream);
				        }
			        }
			        else
			        {
				        _log.Warning(string.Format(ESBResources.MessageNotHandledIgnored,
				                                  message.GetType().FullName,
				                                  transportMessage.MessageId));
			        }
		        }
		        else
		        {
			        bus.Events.OnBeforeHandleMessage(this,
			                                         new BeforeHandleMessageEventArgs(
				                                         pipelineEvent,
				                                         transportMessage));

			        InvokeHandler(bus, handler, transportMessage, message, pipelineEvent, message.GetType());

			        bus.Events.OnAfterHandleMessage(this,
			                                        new AfterHandleMessageEventArgs(
				                                        pipelineEvent,
				                                        pipelineEvent.GetWorkQueue(),
				                                        transportMessage));

			        bus.Configuration.MessageHandlerFactory.ReleaseHandler(handler);
		        }
	        }
	        finally
	        {
				pipelineEvent.GetServiceBus().TransportMessageHandled(); 
	        }
        }
    }
}