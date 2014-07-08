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
		private const int MillisecondsToDefer = 5000; // give the service bus enough time to start up

		private readonly ILog _log;

		public DeferredFixture()
		{
			_log = Log.For(this);
		}

		protected void TestDeferredProcessing(string workQueueUriFormat, string deferredQueueUriFormat,
		                                      string errorQueueUriFormat, bool isTransactional)
		{
			const int deferredMessageCount = 200;
			var configuration = GetInboxConfiguration(workQueueUriFormat, deferredQueueUriFormat, errorQueueUriFormat, 5, isTransactional);

			var module = new DeferredMessageModule(deferredMessageCount);

			configuration.Modules.Add(module);

			using (var bus = new ServiceBus(configuration))
			{
				bus.Start();

				var ignoreTillDate = DateTime.Now.AddMilliseconds(MillisecondsToDefer);

				for (var i = 0; i < deferredMessageCount; i++)
				{
					EnqueueDeferredMessage(bus, ignoreTillDate);	
				}

				var timeout = DateTime.Now.AddMilliseconds(MillisecondsToDefer + 15000);
				// add the extra time else there is no time to process

				// wait for the message to be returned from the deferred queue
				while ((!module.AllDeferredMessageReturned() || !module.AllMessagesHandled())
				       &&
				       timeout > DateTime.Now)
				{
					Thread.Sleep(5);
				}

				_log.Information(string.Format("{0} of {1} deferred messages returned to the inbox.",
				                               module.NumberOfDeferredMessagesReturned, deferredMessageCount));
				_log.Information(string.Format("{0} of {1} deferred messages handled.", module.NumberOfMessagesHandled,
				                               deferredMessageCount));

				Assert.IsTrue(module.AllDeferredMessageReturned(), "All the deferred messages were not returned.");
				Assert.IsTrue(module.AllMessagesHandled(), "All the deferred messages were not handled.");

				Assert.IsTrue(configuration.Inbox.ErrorQueue.IsEmpty());
				Assert.IsNull(configuration.Inbox.DeferredQueue.GetMessage());
				Assert.IsNull(configuration.Inbox.WorkQueue.GetMessage());
			}

			AttemptDropQueues(workQueueUriFormat, errorQueueUriFormat);
		}

		private void EnqueueDeferredMessage(IServiceBus bus, DateTime ignoreTillDate)
		{
			var message = bus.CreateTransportMessage(new SimpleCommand(),
			                                         c => c
				                                              .Defer(ignoreTillDate)
				                                              .WithRecipient(bus.Configuration.Inbox.WorkQueue));

			bus.Configuration.Inbox.WorkQueue.Enqueue(message.MessageId, bus.Configuration.Serializer.Serialize(message));

			_log.Information(string.Format("[message enqueued] : message id = '{0}' / deferred till date = '{1}'",
			                               message.MessageId, message.IgnoreTillDate));
		}

		private static ServiceBusConfiguration GetInboxConfiguration(string workQueueUriFormat, string deferredQueueUriFormat,
		                                                             string errorQueueUriFormat, int threadCount,
		                                                             bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			var inboxWorkQueue =
				configuration.QueueManager.GetQueue(string.Format(workQueueUriFormat, "test-inbox-work"));
			var inboxDeferredQueue =
				configuration.QueueManager.GetQueue(string.Format(deferredQueueUriFormat, "test-inbox-deferred"));
			var errorQueue = configuration.QueueManager.GetQueue(string.Format(errorQueueUriFormat, "test-error"));

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = inboxWorkQueue,
						DeferredQueue = inboxDeferredQueue,
						ErrorQueue = errorQueue,
						DurationToSleepWhenIdle = new[] {TimeSpan.FromMilliseconds(5)},
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