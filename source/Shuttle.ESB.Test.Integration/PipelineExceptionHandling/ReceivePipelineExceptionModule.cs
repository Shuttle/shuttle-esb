using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Guard = Shuttle.Core.Infrastructure.Guard;

namespace Shuttle.ESB.Test.Integration.Core
{
	public class ReceivePipelineExceptionModule :
		IModule,
		IPipelineObserver<OnGetMessage>,
		IPipelineObserver<OnAfterGetMessage>,
		IPipelineObserver<OnDeserializeTransportMessage>,
		IPipelineObserver<OnAfterDeserializeTransportMessage>
	{
		private static readonly object _padlock = new object();

		private readonly IQueue _inboxWorkQueue;

		private readonly List<ExceptionAssertion> _assertions = new List<ExceptionAssertion>();
		private string _assertionName;
		private volatile bool _failed;
		private int _pipelineCount;

		private readonly ILog _log;

		public ReceivePipelineExceptionModule(IQueue inboxWorkQueue)
		{
			Guard.AgainstNull(inboxWorkQueue, "_inboxWorkQueue");

			_inboxWorkQueue = inboxWorkQueue;

			_log = Log.For(this);
		}

		public void Initialize(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			bus.Events.PipelineCreated += PipelineCreated;
			bus.Events.PipelineReleased += PipelineReleased;
			bus.Events.PipelineObtained += PipelineObtained;

			//AddAssertion("BeforeDequeueStream", Assertion);
			AddAssertion("OnGetMessage");
			AddAssertion("OnAfterGetMessage");
			AddAssertion("OnDeserializeTransportMessage");
			AddAssertion("OnAfterDeserializeTransportMessage");
			//AddAssertion("AfterMessageDeserialization", Assertion);
			//AddAssertion("BeforeEnqueueStream", Assertion);
			//AddAssertion("AfterEnqueueStream", Assertion);
			//AddAssertion("BeforeHandleMessage", Assertion);
			//AddAssertion("AfterHandleMessage", Assertion);
			//AddAssertion("BeforeRemoveMessage", Assertion);
			//AddAssertion("AfterRemoveMessage", Assertion);

			//bus.Events.BeforeDequeueStream += (sender, e) => ThrowException("BeforeDequeueStream");
			//bus.Events.AfterDequeueStream += (sender, e) => ThrowException("AfterDequeueStream");
			//bus.Events.AfterMessageDeserialization += (sender, e) => ThrowException("AfterMessageDeserialization");
			//bus.Events.BeforeEnqueueStream += (sender, e) => ThrowException("BeforeEnqueueStream");
			//bus.Events.AfterEnqueueStream += (sender, e) => ThrowException("AfterEnqueueStream");
			//bus.Events.BeforeHandleMessage += (sender, e) => ThrowException("BeforeHandleMessage");
			//bus.Events.AfterHandleMessage += (sender, e) => ThrowException("AfterHandleMessage");
			//bus.Events.BeforeRemoveMessage += (sender, e) => ThrowException("BeforeRemoveMessage");
			//bus.Events.AfterRemoveMessage += (sender, e) => ThrowException("AfterRemoveMessage");

		}

		private void PipelineObtained(object sender, PipelineEventArgs e)
		{
			_pipelineCount += 1;
			_assertionName = string.Empty;

			_log.Information(string.Format("[pipeline obtained] : count = {0}", _pipelineCount));
		}

		private void PipelineCreated(object sender, PipelineEventArgs e)
		{
			if (!e.Pipeline.GetType().FullName.Equals(typeof(InboxMessagePipeline).FullName, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			e.Pipeline.RegisterObserver(this);
		}

		private void ThrowException(string name)
		{
			lock (_padlock)
			{
				_assertionName = name;

				var assertion = GetAssertion(_assertionName);

				if (assertion.HasRun)
				{
					return;
				}

				throw new AssertionException(string.Format("Testing assertion for '{0}'.", name));
			}
		}

		private void AddAssertion(string name)
		{
			lock (_padlock)
			{
				_assertions.Add(new ExceptionAssertion(name));

				_log.Information(string.Format("[added] : assertion = '{0}'.", name));
			}
		}

		private void PipelineReleased(object sender, PipelineEventArgs e)
		{
			if (string.IsNullOrEmpty(_assertionName))
			{
				return;
			}

			lock (_padlock)
			{
				var assertion = GetAssertion(_assertionName);

				if (assertion == null || assertion.HasRun)
				{
					return;
				}

				_log.Information(string.Format("[invoking] : assertion = '{0}'.", assertion.Name));

				try
				{
					// IsEmpty does not work for prefetch queues
					var receivedMessage = _inboxWorkQueue.GetMessage();

					Assert.IsNotNull(receivedMessage);

					_inboxWorkQueue.Release(receivedMessage.AcknowledgementToken);
				}
				catch (Exception ex)
				{
					_log.Error(ex.AllMessages());

					_failed = true;
				}

				assertion.MarkAsRun();

				_log.Information(string.Format("[invoke complete] : assertion = '{0}'.", assertion.Name));
			}
		}

		private ExceptionAssertion GetAssertion(string name)
		{
			return _assertions.Find(item => item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public bool ShouldWait()
		{
			lock (_padlock)
			{
				return !_failed && _assertions.Find(item => !item.HasRun) != null;
			}
		}

		public void Execute(OnGetMessage pipelineEvent1)
		{
			ThrowException("OnGetMessage");
		}

		public void Execute(OnAfterGetMessage pipelineEvent)
		{
			ThrowException("OnAfterGetMessage");
		}

		public void Execute(OnDeserializeTransportMessage pipelineEvent1)
		{
			ThrowException("OnDeserializeTransportMessage");
		}

		public void Execute(OnAfterDeserializeTransportMessage pipelineEvent1)
		{
			ThrowException("OnAfterDeserializeTransportMessage");
		}
	}
}