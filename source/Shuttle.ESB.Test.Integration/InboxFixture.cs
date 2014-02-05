using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.RabbitMQ;
using Shuttle.ESB.Test.Integration.Core;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Integration
{
	public abstract class InboxFixture : IntegrationFixture
	{
		protected event EventHandler<ConfigurationAvailableEventArgs> ConfigurationAvailable = delegate { };

		protected void TestInboxThroughput(string workQueueUriFormat, int timeoutMilliseconds, int count, bool isTransactional)
		{
			TestInboxThroughput(workQueueUriFormat, workQueueUriFormat, timeoutMilliseconds, count, isTransactional);
		}

		protected void TestInboxThroughput(string workQueueUriFormat, string errorQueueUriFormat, int timeoutMilliseconds, int count, bool isTransactional)
		{
			const int threadCount = 1;
			var padlock = new object();
			var configuration = GetTestInboxConfiguration(workQueueUriFormat, errorQueueUriFormat, threadCount, isTransactional);

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

				idleThreads.Clear();
				bus.Start();

				sw.Reset();
				sw.Start();

				while (idleThreads.Count < threadCount)
				{
					Thread.Sleep(25);
				}

				sw.Stop();
			}

			AttemptDropQueues(workQueueUriFormat, errorQueueUriFormat);

			var ms = sw.ElapsedMilliseconds;

			Console.WriteLine("Processed {0} messages in {1} ms", count, ms);

			Assert.IsTrue(ms < timeoutMilliseconds,
						  "Should be able to process at least {0} messages in {1} ms but it ook {2} ms.",
						  count, timeoutMilliseconds, ms);
		}

		private void AttemptDropQueues(string workQueueUriFormat, string errorQueueUriFormat)
		{
			using (var queueManager = QueueManager.Default())
			{
				queueManager.GetQueue(string.Format(workQueueUriFormat, "test-inbox-work")).AttemptDrop();
				queueManager.GetQueue(string.Format(workQueueUriFormat, "test-error")).AttemptDrop();
			}
		}

		protected void TestInboxError(string workQueueUriFormat, bool isTransactional)
		{
			TestInboxError(workQueueUriFormat, workQueueUriFormat, isTransactional);
		}

		protected void TestInboxError(string workQueueUriFormat, string errorQueueUriFormat, bool isTransactional)
		{
			var padlock = new object();
			var configuration = GetTestInboxConfiguration(workQueueUriFormat, errorQueueUriFormat, 1, isTransactional);

			ConfigurationAvailable.Invoke(this, new ConfigurationAvailableEventArgs(configuration));

			using (var bus = new ServiceBus(configuration))
			{
				var message = bus.CreateTransportMessage(new NoHandlerCommand());

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

				Assert.NotNull(configuration.Inbox.ErrorQueue.Dequeue());
			}

			AttemptDropQueues(workQueueUriFormat, errorQueueUriFormat);
		}

		private IServiceBusConfiguration GetTestInboxConfiguration(string workQueueUriFormat, string errorQueueUriFormat, int threadCount, bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			ConfigurationAvailable.Invoke(this, new ConfigurationAvailableEventArgs(configuration));

			var inboxWorkQueue =
				configuration.QueueManager.GetQueue(string.Format(workQueueUriFormat, "test-inbox-work"));
			var errorQueue = configuration.QueueManager.GetQueue(string.Format(workQueueUriFormat, "test-error"));

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

		protected void TestInboxConcurrency(string workQueueUriFormat, int msToComplete, bool isTransactional)
		{
			TestInboxConcurrency(workQueueUriFormat, workQueueUriFormat, msToComplete, isTransactional);
		}

		protected void TestInboxConcurrency(string workQueueUriFormat, string errorQueueUriFormat, int msToComplete, bool isTransactional)
		{
			const int COUNT = 1;

			var padlock = new object();
			var afterDequeueDate = new List<DateTime>();
			var offsetDate = DateTime.MinValue;

			var configuration = GetTestInboxConfiguration(workQueueUriFormat, errorQueueUriFormat, COUNT, isTransactional);

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

			AttemptDropQueues(workQueueUriFormat, errorQueueUriFormat);

			Assert.AreEqual(COUNT, afterDequeueDate.Count,
							string.Format("Dequeued {0} messages but {1} were sent.", afterDequeueDate.Count, COUNT));

			foreach (var dateTime in afterDequeueDate)
			{
				Assert.IsTrue(dateTime.Subtract(offsetDate) < TimeSpan.FromMilliseconds(msToComplete),
							  "All dequeued messages have to be within {0} ms of first dequeue.", msToComplete);
			}
		}

		protected void TestInboxDeferred(string workQueueUriFormat)
		{
			TestInboxDeferred(workQueueUriFormat, workQueueUriFormat);
		}

		protected void TestInboxDeferred(string workQueueUriFormat, string errorQueueUriFormat)
		{
			var configuration = GetTestInboxConfiguration(workQueueUriFormat, errorQueueUriFormat, 1, false);

			ConfigurationAvailable.Invoke(this, new ConfigurationAvailableEventArgs(configuration));

			var messageId = Guid.Empty;
			var messageType = typeof(ReceivePipelineCommand).FullName;

			using (var bus = new ServiceBus(configuration))
			{
				var received = false;

				bus.Events.AfterTransportMessageDeserialization +=
					(sender, eventArgs) =>
					{
						Assert.True(messageId.Equals(eventArgs.TransportMessage.MessageId));
						Assert.True(messageType.Equals(eventArgs.TransportMessage.MessageType, StringComparison.OrdinalIgnoreCase));
						received = true;
					};

				bus.Start();

				var transportMessage = bus.SendDeferred(DateTime.Now.AddMilliseconds(500), new ReceivePipelineCommand(), configuration.Inbox.WorkQueue);
				var timeout = DateTime.Now.AddMilliseconds(1000);

				Assert.IsNotNull(transportMessage);

				messageId = transportMessage.MessageId;

				while (!received && DateTime.Now < timeout)
				{
					Thread.Sleep(5);
				}

				Assert.IsTrue(received);
			}
		}
	}
}