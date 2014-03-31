using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Integration.Idempotence.SqlServer.Msmq
{
	public class DeferredFixture : IntegrationFixture
	{
		protected void TestDeferredProcessing(string workQueueUriFormat, string deferredQueueUriFormat, string errorQueueUriFormat, bool isTransactional)
		{
			var configuration = GetInboxConfiguration(workQueueUriFormat, deferredQueueUriFormat, errorQueueUriFormat, 1, isTransactional);
			var padlock = new object();

			using (var bus = new ServiceBus(configuration))
			{
				var message = bus.CreateTransportMessage(new NoHandlerCommand());

				message.IgnoreTillDate = DateTime.Now.AddMilliseconds(500);
				message.RecipientInboxWorkQueueUri = configuration.Inbox.WorkQueue.Uri.ToString();

				configuration.Inbox.WorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));

				var idleThreads = new List<int>();

				bus.Events.ThreadWaiting += (sender, args) =>
					{
						lock (padlock)
						{
							if (idleThreads.Contains(Thread.CurrentThread.ManagedThreadId))
							{
								return;
							}

							idleThreads.Add(Thread.CurrentThread.ManagedThreadId);
						}
					};

				bus.Start();

				while (idleThreads.Count < 1)
				{
					Thread.Sleep(5);
				}

				lock (padlock)
				{
					while (configuration.Inbox.DeferredQueue.Count() > 0)
					{
						Thread.Sleep(5);
					}

					idleThreads.Clear();
				}

				while (idleThreads.Count < 1)
				{
					Thread.Sleep(5);
				}

				Assert.IsNull(configuration.Inbox.ErrorQueue.GetMessage());
				Assert.IsNull(configuration.Inbox.DeferredQueue.GetMessage());
				Assert.IsNull(configuration.Inbox.WorkQueue.GetMessage());
			}

			AttemptDropQueues(workQueueUriFormat, errorQueueUriFormat);
		}

		private static ServiceBusConfiguration GetInboxConfiguration(string workQueueUriFormat, string deferredQueueUriFormat, string errorQueueUriFormat, int threadCount, bool isTransactional)
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