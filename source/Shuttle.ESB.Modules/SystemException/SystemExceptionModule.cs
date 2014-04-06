using System.Collections.Generic;
using System.Linq;
using System.Net;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.ESB.Modules.Extensions;

namespace Shuttle.ESB.Modules
{
	public class SystemExceptionModule : IModule
	{
		private string hostName;
		private string[] ipAddresses;

		private IServiceBus _bus;

		private static readonly object padlock = new object();
		private readonly List<object> deferredEvents = new List<object>();

		public void Initialize(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			_bus = bus;

			bus.Events.HandlerException += HandlerException;
			bus.Events.AfterPipelineExceptionHandled += PipelineException;

			hostName = Dns.GetHostName();
			ipAddresses = Dns.GetHostAddresses(hostName).Select(address => address.ToString()).ToArray();
		}

		private void PipelineException(object sender, PipelineExceptionEventArgs e)
		{
			var @event = ExceptionEventExtensions.CorePipelineExceptionEvent(e.Pipeline.Exception, hostName, ipAddresses);

			@event.PipelineTypeFullName = e.Pipeline.GetType().FullName;
			@event.PipelineStageName = e.Pipeline.StageName;
			@event.PipelineEventTypeFullName = e.Pipeline.Event.GetType().FullName;

			lock(padlock)
			{
				foreach (var deferredEvent in deferredEvents)
				{
					_bus.Publish(deferredEvent);
				}

				deferredEvents.Clear();
			}

			_bus.Publish(@event);
		}

		private void HandlerException(object sender, HandlerExceptionEventArgs e)
		{
			var @event = ExceptionEventExtensions.CoreHandlerExceptionEvent(e.Exception, hostName, ipAddresses);

			@event.HandlerTypeFullName = e.MessageHandler.GetType().FullName;
			@event.MessageId = e.TransportMessage.MessageId;
			@event.MessageTypeFullName = e.TransportMessage.MessageType;
			@event.WorkQueueUri = e.WorkQueue != null ? e.WorkQueue.Uri.ToString() : string.Empty;
			@event.ErrorQueueUri = e.ErrorQueue != null ? e.ErrorQueue.Uri.ToString() : string.Empty;
			@event.RetryCount = e.TransportMessage.FailureMessages.Count() + 1;
			@event.MaximumFailureCount = e.PipelineEvent.Pipeline.State.GetServiceBus().Configuration.Inbox.MaximumFailureCount;

			// cannot publish here since handler is wrapped in a transaction scope
			// will always also result in pipeline exception so publish there
			lock(padlock)
			{
				deferredEvents.Add(@event);
			}
		}
	}
}