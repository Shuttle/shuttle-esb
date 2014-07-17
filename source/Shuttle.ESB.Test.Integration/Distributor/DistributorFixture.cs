using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class DistributorFixture : IntegrationFixture
	{
		private readonly ILog _log;

		public DistributorFixture()
		{
			_log = Log.For(this);
		}

		protected void TestDeferredProcessing(string queueUriFormat, bool isTransactional)
		{
			var configuration = GetDistributorConfiguration(queueUriFormat, isTransactional);

			using (var bus = new ServiceBus(configuration))
			{
				for (var i = 0; i < 5; i++)
				{
					var command = new SimpleCommand
					{
						Name = Guid.NewGuid().ToString()
					};

					var workQueue = bus.Configuration.Inbox.WorkQueue;
					var message = bus.CreateTransportMessage(command, c => c.WithRecipient(workQueue));

					workQueue.Enqueue(message.MessageId, bus.Configuration.Serializer.Serialize(message));
				}

				bus.Start();


			}

			AttemptDropQueues(queueUriFormat);
		}

		private static ServiceBusConfiguration GetDistributorConfiguration(string queueUriFormat, bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			var errorQueue = configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-error"));

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-distributor-work")),
						ErrorQueue = errorQueue,
						DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
						ThreadCount = 1,
						Distribute = true,
						DistributeSendCount = 3
					};

			configuration.ControlInbox = new ControlInboxQueueConfiguration
				{
					WorkQueue = configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-distributor-control")),
					ErrorQueue = errorQueue,
					DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
					ThreadCount = 1
				};

			configuration.Inbox.WorkQueue.AttemptDrop();
			configuration.ControlInbox.WorkQueue.AttemptDrop();
			errorQueue.AttemptDrop();

			configuration.QueueManager.CreatePhysicalQueues(configuration);

			configuration.Inbox.WorkQueue.AttemptPurge();
			configuration.ControlInbox.WorkQueue.AttemptPurge();
			errorQueue.AttemptPurge();

			return configuration;
		}

		private static ServiceBusConfiguration GetWorkerConfiguration(string queueUriFormat, bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			var inboxWorkQueue =
				configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-worker-work"));
			var errorQueue = configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-error"));

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = inboxWorkQueue,
						ErrorQueue = errorQueue,
						DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
						ThreadCount = 1
					};

			configuration.Worker = new WorkerConfiguration(configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-distributor-control")), 30);

			inboxWorkQueue.AttemptDrop();
			errorQueue.AttemptDrop();

			configuration.QueueManager.CreatePhysicalQueues(configuration);

			inboxWorkQueue.AttemptPurge();
			errorQueue.AttemptPurge();

			return configuration;
		}
	}
}