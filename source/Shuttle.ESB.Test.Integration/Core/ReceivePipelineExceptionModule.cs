using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Guard = Shuttle.Core.Infrastructure.Guard;

namespace Shuttle.ESB.Test.Integration.Core
{
	public delegate void LogMessageDelegate(string message);

	public class ReceivePipelineExceptionModule : IModule
	{
		private static readonly object padlock = new object();

		private readonly IQueue inboxWorkQueue;

		private readonly List<ExceptionAssertion> assertions = new List<ExceptionAssertion>();
		private string assertionName;
		private volatile bool failed;

		private readonly ILog log;

		public ReceivePipelineExceptionModule(IQueue inboxWorkQueue)
		{
			Guard.AgainstNull(inboxWorkQueue, "inboxWorkQueue");

			this.inboxWorkQueue = inboxWorkQueue;

			log = Log.For(this);
		}

		public void Initialize(IServiceBus serviceBus)
		{
			Guard.AgainstNull(serviceBus, "serviceBus");

			AddAssertion("BeforeDequeueStream", Assertion);
			AddAssertion("AfterDequeueStream", Assertion);
			AddAssertion("AfterMessageDeserialization", Assertion);
			AddAssertion("BeforeEnqueueStream", Assertion);
			AddAssertion("AfterEnqueueStream", Assertion);
			AddAssertion("BeforeHandleMessage", Assertion);
			AddAssertion("AfterHandleMessage", Assertion);
			AddAssertion("BeforeRemoveMessage", Assertion);
			AddAssertion("AfterRemoveMessage", Assertion);

			serviceBus.Events.BeforeDequeueStream += (sender, e) => ThrowException("BeforeDequeueStream");
			serviceBus.Events.AfterDequeueStream += (sender, e) => ThrowException("AfterDequeueStream");
			serviceBus.Events.AfterMessageDeserialization += (sender, e) => ThrowException("AfterMessageDeserialization");
			serviceBus.Events.BeforeEnqueueStream += (sender, e) => ThrowException("BeforeEnqueueStream");
			serviceBus.Events.AfterEnqueueStream += (sender, e) => ThrowException("AfterEnqueueStream");
			serviceBus.Events.BeforeHandleMessage += (sender, e) => ThrowException("BeforeHandleMessage");
			serviceBus.Events.AfterHandleMessage += (sender, e) => ThrowException("AfterHandleMessage");
			serviceBus.Events.BeforeRemoveMessage += (sender, e) => ThrowException("BeforeRemoveMessage");
			serviceBus.Events.AfterRemoveMessage += (sender, e) => ThrowException("AfterRemoveMessage");

			serviceBus.Events.PipelineReleased += PipelineReleased;
		}

		private void ThrowException(string name)
		{
			assertionName = name;

			lock (padlock)
			{
				var assertion = GetAssertion(assertionName);

				if (assertion.HasRun)
				{
					return;
				}

				throw new AssertionException(string.Format("Testing assertion for '{0}'.", name));
			}
		}

		private void AddAssertion(string name, Action action)
		{
			lock (padlock)
			{
				assertions.Add(new ExceptionAssertion(name, action));

				log.Information(string.Format("Added assertion for '{0}'.", name));
			}
		}

		private void PipelineReleased(object sender, PipelineEventArgs e)
		{
			lock (padlock)
			{
				var assertion = GetAssertion(assertionName);

				if (assertion == null)
				{
					return;
				}

				log.Information(string.Format("Invoking assertion for '{0}'.", assertion.Name));

				assertion.Action.Invoke();

				assertion.MarkAsRun();
			}
		}

		private ExceptionAssertion GetAssertion(string name)
		{
			return assertions.Find(item => item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		private void Assertion()
		{
			RunAssertion(() => Assert.IsFalse(inboxWorkQueue.IsEmpty()));
		}

		private void RunAssertion(Action action)
		{
			try
			{
				action.Invoke();

				log.Information("Assertions OK.");
			}
			catch (Exception ex)
			{
				log.Error(ex.CompactMessages());

				failed = true;
			}
		}

		public bool ShouldWait()
		{
			lock (padlock)
			{
				return !failed && assertions.Find(item => !item.HasRun) != null;
			}
		}
	}
}