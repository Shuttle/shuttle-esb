using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.SqlServer.Idempotence;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Integration.Idempotence.SqlServer.Msmq
{
	public class IdempotenceFixture : IntegrationFixture
	{
		protected void TestFullProcessing(string workQueueUriFormat, string errorQueueUriFormat, bool isTransactional, bool enqueueUniqueMessages)
		{
			const int messageCount = 200;

			var configuration = GetInboxConfiguration(workQueueUriFormat, errorQueueUriFormat, 5, isTransactional);
			var padlock = new object();

			using (var bus = new ServiceBus(configuration))
			{
				if (enqueueUniqueMessages)
				{
					for (int i = 0; i < messageCount; i++)
					{
						var message = bus.CreateTransportMessage(new IdempotenceCommand());

						message.RecipientInboxWorkQueueUri = configuration.Inbox.WorkQueue.Uri.ToString();

						configuration.Inbox.WorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));
					}
				}
				else
				{
					var message = bus.CreateTransportMessage(new IdempotenceCommand());

					message.RecipientInboxWorkQueueUri = configuration.Inbox.WorkQueue.Uri.ToString();

					for (int i = 0; i < messageCount; i++)
					{
						configuration.Inbox.WorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));
					}
				}

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

				while (idleThreads.Count < 5)
				{
					Thread.Sleep(5);
				}

				Assert.IsNull(configuration.Inbox.ErrorQueue.Dequeue());
				Assert.IsNull(configuration.Inbox.WorkQueue.Dequeue());

				if (enqueueUniqueMessages)
				{
					Assert.AreEqual(messageCount, ((IdempotenceMessageHandlerFactory)bus.Configuration.MessageHandlerFactory).ProcessedCount);
				}
				else
				{
					Assert.AreEqual(1, ((IdempotenceMessageHandlerFactory)bus.Configuration.MessageHandlerFactory).ProcessedCount);
				}
			}

			AttemptDropQueues(workQueueUriFormat, errorQueueUriFormat);
		}

		private static ServiceBusConfiguration GetInboxConfiguration(string workQueueUriFormat, string errorQueueUriFormat,
															   int threadCount, bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			configuration.MessageRouteProvider = new IdempotenceMessageRouteProvider();
			configuration.MessageHandlerFactory = new IdempotenceMessageHandlerFactory();
			configuration.IdempotenceService = IdempotenceService.Default();

			var inboxWorkQueue =
				configuration.QueueManager.GetQueue(string.Format(workQueueUriFormat, "test-inbox-work"));
			var errorQueue = configuration.QueueManager.GetQueue(string.Format(errorQueueUriFormat, "test-error"));

			configuration.Inbox =
				new InboxQueueConfiguration
				{
					WorkQueue = inboxWorkQueue,
					ErrorQueue = errorQueue,
					DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
					ThreadCount = threadCount
				};

			inboxWorkQueue.AttemptDrop();
			errorQueue.AttemptDrop();

			configuration.QueueManager.CreatePhysicalQueues(configuration);

			inboxWorkQueue.AttemptPurge();
			errorQueue.AttemptPurge();

			return configuration;
		}
	}
}