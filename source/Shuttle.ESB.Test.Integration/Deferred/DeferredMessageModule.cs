using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class DeferredMessageModule :
		IModule,
		IPipelineObserver<OnAfterHandleMessage>,
		IPipelineObserver<OnAfterProcessDeferredMessage>
	{
		private readonly object padlock =new object();
		private readonly int _deferredMessageCount;
		private readonly ILog _log;

		public DeferredMessageModule(int deferredMessageCount)
		{
			_deferredMessageCount = deferredMessageCount;

			_log = Log.For(this);
		}

		public int NumberOfDeferredMessagesReturned { get; private set; }
		public int NumberOfMessagesHandled { get; private set; }

		public void Initialize(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			bus.Events.PipelineCreated += PipelineCreated;
		}

		private void PipelineCreated(object sender, PipelineEventArgs e)
		{
			if (!e.Pipeline.GetType().FullName.Equals(typeof(InboxMessagePipeline).FullName, StringComparison.InvariantCultureIgnoreCase)
			    &&
			    !e.Pipeline.GetType().FullName.Equals(typeof(DeferredMessagePipeline).FullName, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			e.Pipeline.RegisterObserver(this);
		}

		public void Execute(OnAfterHandleMessage pipelineEvent)
		{
			_log.Information(string.Format("[OnAfterHandleMessage]"));

			lock (padlock)
			{
				NumberOfMessagesHandled++;
			}
		}

		public void Execute(OnAfterProcessDeferredMessage pipelineEvent)
		{
			_log.Information(string.Format("[OnAfterProcessDeferredMessage] : deferred message returned = '{0}'", pipelineEvent.Pipeline.State.GetDeferredMessageReturned()));

			if (pipelineEvent.Pipeline.State.GetDeferredMessageReturned())
			{
				lock (padlock)
				{
					NumberOfDeferredMessagesReturned++;
				}
			}
		}

		public bool AllMessagesHandled()
		{
			return NumberOfMessagesHandled == _deferredMessageCount;
		}

		public bool AllDeferredMessageReturned()
		{
			return NumberOfDeferredMessagesReturned == _deferredMessageCount;
		}
	}
}