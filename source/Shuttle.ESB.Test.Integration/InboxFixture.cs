using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Integration
{
	public class InboxFixture : IntegrationFixture
	{
		public class ConfigurationEventArgs : EventArgs
		{
			public ServiceBusConfiguration Configuration { get; private set; }

			public ConfigurationEventArgs(ServiceBusConfiguration configuration)
			{
				Configuration = configuration;
			}
		}

		public event EventHandler<ConfigurationEventArgs> ConfigurationComplete = delegate { };

		protected void TestInboxThroughput(string queueSchemeAndHost, int count, int timeoutMilliseconds, bool useJournal, bool isTransactional)
		{
			var configuration = GetTestInboxConfiguration(queueSchemeAndHost, useJournal, 1, isTransactional);

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

			DropQueues(queueSchemeAndHost, useJournal);
		}

		private void DropQueues(string queueSchemeAndHost, bool useJournal)
		{
			var inboxWorkQueue =
				QueueManager.Instance.GetQueue(string.Format("{0}/test-inbox-work", queueSchemeAndHost)) as IDrop;
			var inboxJournalQueue = useJournal
										? QueueManager.Instance.GetQueue(string.Format("{0}/test-inbox-journal",
																					   queueSchemeAndHost)) as IDrop
										: null;
			var errorQueue = QueueManager.Instance.GetQueue(string.Format("{0}/test-error", queueSchemeAndHost)) as IDrop;

			if (inboxWorkQueue != null)
			{
				inboxWorkQueue.Drop();
			}

			if (inboxJournalQueue != null)
			{
				inboxJournalQueue.Drop();
			}

			if (errorQueue != null)
			{
				errorQueue.Drop();
			}
		}

		protected void TestInboxError(string queueSchemeAndHost, bool useJournal, bool isTransactional)
		{
			var configuration = GetTestInboxConfiguration(queueSchemeAndHost, useJournal, 1, isTransactional);

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

			DropQueues(queueSchemeAndHost, useJournal);
		}

		private IServiceBusConfiguration GetTestInboxConfiguration(string queueSchemeAndHost, bool useJournal, int threadCount,
																   bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			var inboxWorkQueue =
				QueueManager.Instance.GetQueue(string.Format("{0}/test-inbox-work", queueSchemeAndHost));
			var inboxJournalQueue = useJournal
										? QueueManager.Instance.GetQueue(string.Format("{0}/test-inbox-journal",
																					   queueSchemeAndHost))
										: null;
			var errorQueue = QueueManager.Instance.GetQueue(string.Format("{0}/test-error", queueSchemeAndHost));

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = inboxWorkQueue,
						JournalQueue = inboxJournalQueue,
						ErrorQueue = errorQueue,
						DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
						ThreadCount = threadCount
					};


			QueueManager.Instance.CreatePhysicalQueues(configuration, QueueCreationType.All);

			inboxWorkQueue.Purge();
			errorQueue.Purge();

			if (useJournal)
			{
				inboxJournalQueue.Purge();
			}

			ConfigurationComplete.Invoke(this, new ConfigurationEventArgs(configuration));

			return configuration;
		}

		protected void TestInboxConcurrency(string queueSchemeAndHost, bool useJournal, int msToComplete, bool isTransactional)
		{
			const int COUNT = 10;

			var padlock = new object();
			var configuration = GetTestInboxConfiguration(queueSchemeAndHost, useJournal, COUNT, isTransactional);
			var afterDequeueDate = new List<DateTime>();
			var offsetDate = DateTime.MinValue;

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

				var idleCount = 0;

				bus.Events.ThreadWaiting += (sender, args) =>
					{
						lock (padlock)
						{
							idleCount++;
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

				while (idleCount < COUNT)
				{
					Thread.Sleep(50);
				}
			}

			Assert.AreEqual(COUNT, afterDequeueDate.Count,
							string.Format("Dequeued {0} messages but {1} were sent.", afterDequeueDate.Count, COUNT));

			foreach (var dateTime in afterDequeueDate)
			{
				Assert.IsTrue(dateTime.Subtract(offsetDate) < TimeSpan.FromMilliseconds(msToComplete),
							  "All dequeued messages have to be within {0} ms of first dequeue.", msToComplete);
			}

			DropQueues(queueSchemeAndHost, useJournal);
		}
	}
}