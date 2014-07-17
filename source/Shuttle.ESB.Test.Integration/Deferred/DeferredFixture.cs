using System;
using System.Threading;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class DeferredFixture : IntegrationFixture
	{
		private readonly ILog _log;

		public DeferredFixture()
		{
			_log = Log.For(this);
		}

		protected void TestDeferredProcessing(string queueUriFormat, bool isTransactional)
		{
			const int deferredMessageCount = 10;
			const int millisecondsToDefer = 500;

			var configuration = GetInboxConfiguration(queueUriFormat, 1, isTransactional);

			var module = new DeferredMessageModule(deferredMessageCount);

			configuration.Modules.Add(module);

			using (var bus = new ServiceBus(configuration))
			{
				bus.Start();

				var ignoreTillDate = DateTime.Now.AddSeconds(5);

				for (var i = 0; i < deferredMessageCount; i++)
				{
					EnqueueDeferredMessage(bus, ignoreTillDate);

					ignoreTillDate = ignoreTillDate.AddMilliseconds(millisecondsToDefer);
				}

				// add the extra time else there is no time to process message being returned
				var timeout = ignoreTillDate.AddSeconds(150);
				var timedOut = false;

				_log.Information(string.Format("[start wait] : now = '{0}'", DateTime.Now));

				// wait for the message to be returned from the deferred queue
				while (!module.AllMessagesHandled()
					   &&
					   !timedOut)
				{
					Thread.Sleep(millisecondsToDefer);

					timedOut = timeout < DateTime.Now;
				}

				_log.Information(string.Format("[end wait] : now = '{0}' / timeout = '{1}' / timed out = '{2}'", DateTime.Now, timeout, timedOut));

				_log.Information(string.Format("{0} of {1} deferred messages returned to the inbox.",
											   module.NumberOfDeferredMessagesReturned, deferredMessageCount));
				_log.Information(string.Format("{0} of {1} deferred messages handled.", module.NumberOfMessagesHandled,
											   deferredMessageCount));

				Assert.IsTrue(module.AllMessagesHandled(), "All the deferred messages were not handled.");

				Assert.IsTrue(configuration.Inbox.ErrorQueue.IsEmpty());
				Assert.IsNull(configuration.Inbox.DeferredQueue.GetMessage());
				Assert.IsNull(configuration.Inbox.WorkQueue.GetMessage());
			}

			AttemptDropQueues(queueUriFormat);
		}

		private void EnqueueDeferredMessage(IServiceBus bus, DateTime ignoreTillDate)
		{
			var command = new SimpleCommand
				{
					Name = Guid.NewGuid().ToString()
				};

			var message = bus.CreateTransportMessage(command, c => c.Defer(ignoreTillDate)
						 .WithRecipient(bus.Configuration.Inbox.WorkQueue));

			bus.Configuration.Inbox.WorkQueue.Enqueue(message.MessageId, bus.Configuration.Serializer.Serialize(message));

			_log.Information(string.Format("[message enqueued] : name = '{0}' / deferred till date = '{1}'", command.Name, message.IgnoreTillDate));
		}

		private static ServiceBusConfiguration GetInboxConfiguration(string queueUriFormat, int threadCount, bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			var inboxWorkQueue = configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-inbox-work"));
			var inboxDeferredQueue = configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-inbox-deferred"));
			var errorQueue = configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-error"));

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = inboxWorkQueue,
						DeferredQueue = inboxDeferredQueue,
						ErrorQueue = errorQueue,
						DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
						ThreadCount = threadCount
					};

			inboxWorkQueue.AttemptDrop();
			inboxDeferredQueue.AttemptDrop();
			errorQueue.AttemptDrop();

			configuration.QueueManager.CreatePhysicalQueues(configuration);

			inboxWorkQueue.AttemptPurge();
			inboxDeferredQueue.AttemptPurge();
			errorQueue.AttemptPurge();

			return configuration;
		}
	}
}