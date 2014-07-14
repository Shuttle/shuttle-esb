using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.SqlServer.Idempotence;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Integration.Idempotence.SqlServer.Msmq
{
	public class IdempotenceFixture : IntegrationFixture
	{
		protected void TestIdempotenceProcessing(string queueUriFormat, string errorQueueUriFormat, bool isTransactional,
		                                         bool enqueueUniqueMessages)
		{
			const int threadCount = 1;
			const int messageCount = 5;

			var configuration = GetInboxConfiguration(queueUriFormat, errorQueueUriFormat, threadCount, isTransactional);
			var padlock = new object();

			using (var bus = new ServiceBus(configuration))
			{
				if (enqueueUniqueMessages)
				{
					for (var i = 0; i < messageCount; i++)
					{
						var message = bus.CreateTransportMessage(new IdempotenceCommand(), c => c.WithRecipient(configuration.Inbox.WorkQueue));

						configuration.Inbox.WorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));
					}
				}
				else
				{
					var message = bus.CreateTransportMessage(new IdempotenceCommand(), c => c.WithRecipient(configuration.Inbox.WorkQueue));

					for (var i = 0; i < messageCount; i++)
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

				while (idleThreads.Count < threadCount)
				{
					Thread.Sleep(5);
				}

				Assert.IsNull(configuration.Inbox.ErrorQueue.GetMessage());
				Assert.IsNull(configuration.Inbox.WorkQueue.GetMessage());

				if (enqueueUniqueMessages)
				{
					Assert.AreEqual(messageCount,
					                ((IdempotenceMessageHandlerFactory) bus.Configuration.MessageHandlerFactory).ProcessedCount);
				}
				else
				{
					Assert.AreEqual(1, ((IdempotenceMessageHandlerFactory) bus.Configuration.MessageHandlerFactory).ProcessedCount);
				}
			}

			AttemptDropQueues(queueUriFormat);
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
						DurationToIgnoreOnFailure = new[] {TimeSpan.FromMilliseconds(5)},
						DurationToSleepWhenIdle = new[] {TimeSpan.FromMilliseconds(5)},
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