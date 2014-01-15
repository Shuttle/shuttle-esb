using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Integration.Core;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Integration
{
	public abstract class InboxFixture : IntegrationFixture
	{
		protected event EventHandler<ConfigurationAvailableEventArgs> ConfigurationAvailable = delegate { }; 

		protected void TestInboxThroughput(string queueSchemeAndHost, int timeoutMilliseconds, int count,
										   bool useIdempotenceTracker, bool useJournal, bool isTransactional)
		{
			var configuration = GetTestInboxConfiguration(queueSchemeAndHost, useJournal, 1, isTransactional);

			ConfigurationAvailable.Invoke(this, new ConfigurationAvailableEventArgs(configuration));

			Console.WriteLine("Sending {0} messages to input queue '{1}'.", count, configuration.Inbox.WorkQueue.Uri);

			var sw = new Stopwatch();

			using (var bus = new ServiceBus(configuration))
			{
				for (var i = 0; i < 5; i++)
				{
					var warmup = bus.CreateTransportMessage(new SimpleCommand("warmup"));

					configuration.Inbox.WorkQueue.Enqueue(warmup.MessageId, configuration.Serializer.Serialize(warmup));
				}

				var working = true;

				bus.Events.ThreadWaiting += (sender, args) => { working = false; };

				bus.Start();

				while (working)
				{
					Thread.Sleep(25);
				}

				bus.Stop();

				sw.Start();

				for (var i = 0; i < count; i++)
				{
					var message = bus.CreateTransportMessage(new SimpleCommand("command " + i));

					configuration.Inbox.WorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));
				}

				sw.Stop();

				Console.WriteLine("Took {0} ms to send {1} messages.  Starting processing.", sw.ElapsedMilliseconds,
								  count);

				bus.Start();

				sw.Reset();
				sw.Start();

				working = true;

				while (working)
				{
					Thread.Sleep(25);
				}

				sw.Stop();
			}

			var ms = sw.ElapsedMilliseconds;

			Console.WriteLine("Processed {0} messages in {1} ms", count, ms);

			Assert.IsTrue(ms < timeoutMilliseconds,
						  "Should be able to process at least {0} messages in {1} ms but it ook {2} ms.",
						  count, timeoutMilliseconds, ms);

			AttemptDropQueues(configuration.QueueManager, queueSchemeAndHost, useJournal);
		}

		private void AttemptDropQueues(IQueueManager queueManager, string queueSchemeAndHost, bool useJournal)
		{
			queueManager.GetQueue(string.Format("{0}/test-inbox-work", queueSchemeAndHost)).AttemptDrop();

			if (useJournal)
			{
				queueManager.GetQueue(string.Format("{0}/test-inbox-journal", queueSchemeAndHost)).AttemptDrop();
			}

			queueManager.GetQueue(string.Format("{0}/test-error", queueSchemeAndHost)).AttemptDrop();
		}

		protected void TestInboxError(string queueSchemeAndHost, bool useJournal, bool isTransactional)
		{
			var configuration = GetTestInboxConfiguration(queueSchemeAndHost, useJournal, 1, isTransactional);

			ConfigurationAvailable.Invoke(this, new ConfigurationAvailableEventArgs(configuration));

			using (var bus = new ServiceBus(configuration))
			{
				var message = bus.CreateTransportMessage(new NoHandlerCommand());

				configuration.Inbox.WorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));

				var working = true;

				bus.Events.ThreadWaiting += (sender, args) => { working = false; };

				bus.Start();

				while (working)
				{
					Thread.Sleep(5);
				}
			}

			Assert.NotNull(configuration.Inbox.ErrorQueue.Dequeue());

			AttemptDropQueues(configuration.QueueManager, queueSchemeAndHost, useJournal);
		}

		private IServiceBusConfiguration GetTestInboxConfiguration(string queueSchemeAndHost, bool useJournal, int threadCount,
																   bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			ConfigurationAvailable.Invoke(this, new ConfigurationAvailableEventArgs(configuration));

			var inboxWorkQueue =
				configuration.QueueManager.GetQueue(string.Format("{0}/test-inbox-work", queueSchemeAndHost));
			var inboxJournalQueue = useJournal
										? configuration.QueueManager.GetQueue(string.Format("{0}/test-inbox-journal",
																					   queueSchemeAndHost))
										: null;
			var errorQueue = configuration.QueueManager.GetQueue(string.Format("{0}/test-error", queueSchemeAndHost));

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = inboxWorkQueue,
						JournalQueue = inboxJournalQueue,
						ErrorQueue = errorQueue,
						DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
						ThreadCount = threadCount
					};

			inboxWorkQueue.AttemptDrop();
			inboxJournalQueue.AttemptDrop();
			errorQueue.AttemptDrop();

			configuration.QueueManager.CreatePhysicalQueues(configuration, QueueCreationType.All);

			inboxWorkQueue.AttemptPurge();
			inboxJournalQueue.AttemptPurge();
			errorQueue.AttemptPurge();

			if (useJournal)
			{
				inboxJournalQueue.Purge();
			}

			return configuration;
		}

		protected void TestInboxConcurrency(string queueSchemeAndHost, int msToComplete, bool useJournal, bool isTransactional)
		{
			const int COUNT = 1;

			var padlock = new object();
			var afterDequeueDate = new List<DateTime>();
			var offsetDate = DateTime.MinValue;

			var configuration = GetTestInboxConfiguration(queueSchemeAndHost, useJournal, COUNT, isTransactional);

			ConfigurationAvailable.Invoke(this, new ConfigurationAvailableEventArgs(configuration));

			using (var bus = new ServiceBus(configuration))
			{
				for (var i = 0; i < COUNT; i++)
				{
					var message = bus.CreateTransportMessage(new ConcurrentCommand
						{
							MessageIndex = i
						});

					configuration.Inbox.WorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));
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

				bus.Events.AfterDequeueStream += (sender, args) =>
					{
						lock (padlock)
						{
							if (offsetDate == DateTime.MinValue)
							{
								offsetDate = DateTime.Now;

								Console.WriteLine("Offset date: {0}", offsetDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
							}

							var dateTime = DateTime.Now;

							afterDequeueDate.Add(dateTime);

							Console.WriteLine("Dequeued date: {0}", dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
						}
					};

				bus.Start();

				while (idleThreads.Count < COUNT)
				{
					Thread.Sleep(30);
				}
			}

			Assert.AreEqual(COUNT, afterDequeueDate.Count,
							string.Format("Dequeued {0} messages but {1} were sent.", afterDequeueDate.Count, COUNT));

			foreach (var dateTime in afterDequeueDate)
			{
				Assert.IsTrue(dateTime.Subtract(offsetDate) < TimeSpan.FromMilliseconds(msToComplete),
							  "All dequeued messages have to be within {0} ms of first dequeue.", msToComplete);
			}

			AttemptDropQueues(configuration.QueueManager, queueSchemeAndHost, useJournal);
		}

		protected void TestInboxDeferred(string queueSchemeAndHost)
		{
			var configuration = GetTestInboxConfiguration(queueSchemeAndHost, false, 1, false);

			ConfigurationAvailable.Invoke(this, new ConfigurationAvailableEventArgs(configuration));

			var messageId = Guid.Empty;
			var messageType = typeof(ReceivePipelineCommand).FullName;

			using (var bus = new ServiceBus(configuration))
			{
				var id = messageId;
				var type = messageType;
				var waiting = true;

				bus.Events.AfterTransportMessageDeserialization +=
					(sender, eventArgs) =>
					{
						Assert.True(id.Equals(eventArgs.TransportMessage.MessageId));
						Assert.True(type.Equals(eventArgs.TransportMessage.MessageType, StringComparison.OrdinalIgnoreCase));
						waiting = false;
					};

				bus.Events.ThreadWaiting += (sender, args) => { waiting = false; };

				bus.Start();

				Assert.IsNotNull(bus.SendDeferred(DateTime.Now.AddMilliseconds(500), new ReceivePipelineCommand(), configuration.Inbox.WorkQueue).MessageId);

				while (waiting)
				{
					Thread.Sleep(5);
				}
			}
		}
	}
}