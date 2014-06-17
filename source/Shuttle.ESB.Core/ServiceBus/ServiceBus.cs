using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ServiceBus : IServiceBus
	{
		private static bool _started;
		private readonly IMessageSender _messageSender;

		private IProcessorThreadPool _controlThreadPool;
		private IProcessorThreadPool _inboxThreadPool;
		private IProcessorThreadPool _outboxThreadPool;
		private IProcessorThreadPool _deferredMessageThreadPool;

		internal ServiceBus(IServiceBusConfiguration configuration)
		{
			Guard.AgainstNull(configuration, "configuration");

			Configuration = configuration;

			Events = new ServiceBusEvents();

			_messageSender = new MessageSender(this);
		}

		public IServiceBusConfiguration Configuration { get; private set; }
		public IServiceBusEvents Events { get; private set; }

		public IServiceBus Start()
		{
			if (Started)
			{
				throw new ApplicationException(ESBResources.ServiceBusInstanceAlreadyStarted);
			}

			GuardAgainstInvalidConfiguration();

			foreach (var module in Configuration.Modules)
			{
				module.Initialize(this);
			}

			var startupPipeline = new StartupPipeline(this);

			Events.OnPipelineCreated(this, new PipelineEventArgs(startupPipeline));

			startupPipeline.Execute();

			_inboxThreadPool = startupPipeline.State.Get<IProcessorThreadPool>("InboxThreadPool");
			_controlThreadPool = startupPipeline.State.Get<IProcessorThreadPool>("ControlInboxThreadPool");
			_outboxThreadPool = startupPipeline.State.Get<IProcessorThreadPool>("OutboxThreadPool");
			_deferredMessageThreadPool = startupPipeline.State.Get<IProcessorThreadPool>("DeferredMessageThreadPool");

			_started = true;

			return this;
		}

		private void GuardAgainstInvalidConfiguration()
		{
			Guard.Against<ESBConfigurationException>(Configuration.Serializer == null, ESBResources.NoSerializerException);

			Guard.Against<ESBConfigurationException>(Configuration.MessageHandlerFactory == null,
			                                         ESBResources.NoMessageHandlerFactoryException);

			Guard.Against<WorkerException>(Configuration.IsWorker && !Configuration.HasInbox, ESBResources.WorkerRequiresInbox);

			if (Configuration.HasInbox)
			{
				Guard.Against<ESBConfigurationException>(Configuration.Inbox.WorkQueue == null,
				                                         string.Format(ESBResources.RequiredQueueMissing, "Inbox.WorkQueue"));

				Guard.Against<ESBConfigurationException>(Configuration.Inbox.ErrorQueue == null,
				                                         string.Format(ESBResources.RequiredQueueMissing, "Inbox.ErrorQueue"));
			}

			if (Configuration.HasOutbox)
			{
				Guard.Against<ESBConfigurationException>(Configuration.Outbox.WorkQueue == null,
				                                         string.Format(ESBResources.RequiredQueueMissing, "Outbox.WorkQueue"));

				Guard.Against<ESBConfigurationException>(Configuration.Outbox.ErrorQueue == null,
				                                         string.Format(ESBResources.RequiredQueueMissing, "Outbox.ErrorQueue"));
			}

			if (Configuration.HasControlInbox)
			{
				Guard.Against<ESBConfigurationException>(Configuration.ControlInbox.WorkQueue == null,
				                                         string.Format(ESBResources.RequiredQueueMissing, "ControlInbox.WorkQueue"));

				Guard.Against<ESBConfigurationException>(Configuration.ControlInbox.ErrorQueue == null,
				                                         string.Format(ESBResources.RequiredQueueMissing, "ControlInbox.ErrorQueue"));
			}
		}

		public void Stop()
		{
			if (!Started)
			{
				return;
			}

			Configuration.Modules.AttemptDispose();

			if (Configuration.HasInbox)
			{
				_inboxThreadPool.Dispose();
			}

			if (Configuration.HasControlInbox)
			{
				_controlThreadPool.Dispose();
			}

			if (Configuration.HasOutbox)
			{
				_outboxThreadPool.Dispose();
			}

			if (Configuration.HasDeferredQueue)
			{
				_deferredMessageThreadPool.Dispose();
			}

			Configuration.QueueManager.AttemptDispose();

			_started = false;
		}

		public bool Started
		{
			get { return _started; }
		}

		public void Dispose()
		{
			Stop();
		}

		public static IServiceBusConfigurationBuilder Create()
		{
			return new ServiceBusConfigurationBuilder();
		}

		public TransportMessage CreateTransportMessage(object message, Action<TransportMessageConfigurator> configurator)
		{
			return _messageSender.CreateTransportMessage(message, configurator);
		}

		public void Dispatch(TransportMessage transportMessage)
		{
			_messageSender.Dispatch(transportMessage);
		}

		public TransportMessage Send(object message)
		{
			return _messageSender.Send(message);
		}

		public TransportMessage Send(object message, Action<TransportMessageConfigurator> configurator)
		{
			return _messageSender.Send(message, configurator);
		}

		public IEnumerable<TransportMessage> Publish(object message)
		{
			return _messageSender.Publish(message);
		}

		public IEnumerable<TransportMessage> Publish(object message, Action<TransportMessageConfigurator> configurator)
		{
			return _messageSender.Publish(message, configurator);
		}
	}
}