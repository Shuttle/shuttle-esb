using System;
using System.Threading;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class DeferredFixture : IntegrationFixture
	{
		private const int MillisecondsToDefer = 1000; // give the service bus enough time to start up

		protected void TestDeferredProcessing(string workQueueUriFormat, string deferredQueueUriFormat,
		                                      string errorQueueUriFormat, bool isTransactional)
		{
			var configuration = GetInboxConfiguration(workQueueUriFormat, deferredQueueUriFormat, errorQueueUriFormat, 1,
			                                          isTransactional);

			var module = new DeferredMessageModule();

			configuration.Modules.Add(module);

			using (var bus = new ServiceBus(configuration))
			{
				bus.Start();

				var message = bus.CreateTransportMessage(new SimpleCommand());

				message.IgnoreTillDate = DateTime.Now.AddMilliseconds(MillisecondsToDefer);
				message.RecipientInboxWorkQueueUri = configuration.Inbox.WorkQueue.Uri.ToString();

				configuration.Inbox.WorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));

				var timeout = DateTime.Now.AddMilliseconds(MillisecondsToDefer + 5000);

				// wait for the message to be returned from the deferred queue
				while ((!module.DeferredMessageReturned || !module.MessageHandled)
				       &&
				       timeout > DateTime.Now)
				{
					Thread.Sleep(5);
				}

				Assert.IsTrue(module.DeferredMessageReturned, "Deferred message was never returned.");
				Assert.IsTrue(module.MessageHandled, "Deferred message was never handled.");

				Assert.IsTrue(configuration.Inbox.ErrorQueue.IsEmpty());
				Assert.IsNull(configuration.Inbox.DeferredQueue.GetMessage());
				Assert.IsNull(configuration.Inbox.WorkQueue.GetMessage());
			}

			AttemptDropQueues(workQueueUriFormat, errorQueueUriFormat);
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