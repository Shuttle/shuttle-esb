using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Integration
{
	public abstract class OutboxFixture : IntegrationFixture
	{
		protected void TestOutboxSending(string workQueueUriFormat, bool isTransactional)
		{
			TestOutboxSending(workQueueUriFormat, workQueueUriFormat, isTransactional);
		}

		protected void TestOutboxSending(string workQueueUriFormat, string errorQueueUriFormat, bool isTransactional)
		{
			const int count = 100;
			const int threadCount = 3;

			var padlock = new object();
			var configuration = GetConfiguration(workQueueUriFormat, errorQueueUriFormat, threadCount, isTransactional);

			var messageRouteProvider = new Mock<IMessageRouteProvider>();

			var receiverWorkQueueUri = string.Format(workQueueUriFormat, "test-receiver-work");

			messageRouteProvider.Setup(m => m.GetRouteUris(It.IsAny<string>())).Returns(new[] {receiverWorkQueueUri});

			configuration.MessageRouteProvider = messageRouteProvider.Object;

			Console.WriteLine("Sending {0} messages.", count);

			using (var bus = new ServiceBus(configuration))
			{
				for (var i = 0; i < count; i++)
				{
					bus.Send(new SimpleCommand());
				}

				var idleThreads = new List<int>();

				bus.Events.ThreadWaiting += (sender, args) =>
					{
						if (!args.PipelineType.FullName.Equals(typeof (OutboxPipeline).FullName))
						{
							return;
						}

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
					Thread.Sleep(25);
				}
			}

			using (var queueManager = new QueueManager())
			{
				var receiverWorkQueue = queueManager.GetQueue(receiverWorkQueueUri);

				for (var i = 0; i < count; i++)
				{
					var receivedMessage = receiverWorkQueue.GetMessage();

					Assert.IsNotNull(receivedMessage);

					receiverWorkQueue.Acknowledge(receivedMessage.AcknowledgementToken);
				}

				receiverWorkQueue.AttemptDrop();

				var outboxWorkQueue = queueManager.GetQueue(string.Format(workQueueUriFormat, "test-outbox-work"));

				Assert.IsTrue(outboxWorkQueue.IsEmpty());

				outboxWorkQueue.AttemptDrop();

				queueManager.GetQueue(string.Format(errorQueueUriFormat, "test-error")).AttemptDrop();
			}
		}

		private static ServiceBusConfiguration GetConfiguration(string workQueueUriFormat, string errorQueueUriFormat,
		                                                        int threadCount, bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			var outboxWorkQueue =
				configuration.QueueManager.GetQueue(string.Format(workQueueUriFormat, "test-outbox-work"));
			var errorQueue = configuration.QueueManager.GetQueue(string.Format(errorQueueUriFormat, "test-error"));

			configuration.Outbox =
				new OutboxQueueConfiguration
					{
						WorkQueue = outboxWorkQueue,
						ErrorQueue = errorQueue,
						DurationToSleepWhenIdle = new[] {TimeSpan.FromMilliseconds(5)},
						ThreadCount = threadCount
					};

			var receiverWorkQueue =
				configuration.QueueManager.GetQueue(string.Format(workQueueUriFormat, "test-receiver-work"));

			outboxWorkQueue.AttemptDrop();
			receiverWorkQueue.AttemptDrop();
			errorQueue.AttemptDrop();

			outboxWorkQueue.AttemptCreate();
			receiverWorkQueue.AttemptCreate();
			errorQueue.AttemptCreate();

			outboxWorkQueue.AttemptPurge();
			receiverWorkQueue.AttemptPurge();
			errorQueue.AttemptPurge();

			return configuration;
		}
	}
}