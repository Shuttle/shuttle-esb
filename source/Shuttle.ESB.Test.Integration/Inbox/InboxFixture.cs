using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Integration.Core;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Integration
{
	public abstract class InboxFixture : IntegrationFixture
	{
		protected void TestInboxThroughput(string workQueueUriFormat, int timeoutMilliseconds, int count, bool isTransactional)
		{
			TestInboxThroughput(workQueueUriFormat, workQueueUriFormat, timeoutMilliseconds, count, isTransactional);
		}

		protected void TestInboxThroughput(string workQueueUriFormat, string errorQueueUriFormat, int timeoutMilliseconds,
		                                   int count, bool isTransactional)
		{
			const int threadCount = 1;
			var padlock = new object();
			var configuration = GetConfiguration(workQueueUriFormat, errorQueueUriFormat, threadCount, isTransactional);

			Console.WriteLine("Sending {0} messages to input queue '{1}'.", count, configuration.Inbox.WorkQueue.Uri);

			var sw = new Stopwatch();

			using (var bus = new ServiceBus(configuration))
			{
				for (var i = 0; i < 5; i++)
				{
					var warmup = bus.CreateTransportMessage(new SimpleCommand("warmup"),
					                                        c => c.WithRecipient(configuration.Inbox.WorkQueue));

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
					var message = bus.CreateTransportMessage(new SimpleCommand("command " + i),
					                                         c => c.WithRecipient(configuration.Inbox.WorkQueue));

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

		protected void TestInboxError(string workQueueUriFormat, bool isTransactional)
		{
			TestInboxError(workQueueUriFormat, workQueueUriFormat, isTransactional);
		}

		protected void TestInboxError(string workQueueUriFormat, string errorQueueUriFormat, bool isTransactional)
		{
			var padlock = new object();
			var configuration = GetConfiguration(workQueueUriFormat, errorQueueUriFormat, 1, isTransactional);

			using (var bus = new ServiceBus(configuration))
			{
				var message = bus.CreateTransportMessage(new ErrorCommand(),
				                                         c => c.WithRecipient(configuration.Inbox.WorkQueue));

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

				Assert.Null(configuration.Inbox.WorkQueue.GetMessage());
				Assert.NotNull(configuration.Inbox.ErrorQueue.GetMessage());
			}

			AttemptDropQueues(workQueueUriFormat, errorQueueUriFormat);
		}

		private static IServiceBusConfiguration GetConfiguration(string workQueueUriFormat, string errorQueueUriFormat,
		                                                         int threadCount, bool isTransactional)
		{
			var configuration = DefaultConfiguration(isTransactional);

			var inboxWorkQueue =
				configuration.QueueManager.GetQueue(string.Format(workQueueUriFormat, "test-inbox-work"));
			var errorQueue = configuration.QueueManager.GetQueue(string.Format(errorQueueUriFormat, "test-error"));

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = inboxWorkQueue,
						ErrorQueue = errorQueue,
						DurationToSleepWhenIdle = new[] {TimeSpan.FromMilliseconds(5)},
						DurationToIgnoreOnFailure= new[] {TimeSpan.FromMilliseconds(5)},
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

		protected void TestInboxConcurrency(string workQueueUriFormat, string errorQueueUriFormat, int msToComplete,
		                                    bool isTransactional)
		{
			const int threadCount = 1;

			var padlock = new object();
			var configuration = GetConfiguration(workQueueUriFormat, errorQueueUriFormat, threadCount, isTransactional);
			var module = new InboxConcurrencyModule();

			configuration.Modules.Add(module);

			using (var bus = new ServiceBus(configuration))
			{
				for (var i = 0; i < threadCount; i++)
				{
					var message = bus.CreateTransportMessage(new ConcurrentCommand
						{
							MessageIndex = i
						}, c => c.WithRecipient(configuration.Inbox.WorkQueue));

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

				bus.Start();

				while (idleThreads.Count < threadCount)
				{
					Thread.Sleep(30);
				}
			}

			AttemptDropQueues(workQueueUriFormat, errorQueueUriFormat);

			Assert.AreEqual(threadCount, module.OnAfterGetMessageCount,
			                string.Format("Got {0} messages but {1} were sent.", module.OnAfterGetMessageCount, threadCount));

			Assert.IsTrue(module.AllMessagesReceivedWithinTimespan(msToComplete),
			              "All dequeued messages have to be within {0} ms of first get message.", msToComplete);
		}

		protected void TestInboxDeferred(string workQueueUriFormat)
		{
			TestInboxDeferred(workQueueUriFormat, workQueueUriFormat);
		}

		protected void TestInboxDeferred(string workQueueUriFormat, string errorQueueUriFormat)
		{
			var configuration = GetConfiguration(workQueueUriFormat, errorQueueUriFormat, 1, false);

			var module = new InboxDeferredModule();

			configuration.Modules.Add(module);

			Guid messageId;
			var messageType = typeof (ReceivePipelineCommand).FullName;

			using (var bus = new ServiceBus(configuration))
			{
				bus.Start();

				var transportMessage = bus.Send(new ReceivePipelineCommand(), c => c.Defer(DateTime.Now.AddMilliseconds(500))
				                                                                    .WithRecipient(configuration.Inbox.WorkQueue));

				var timeout = DateTime.Now.AddMilliseconds(1000);

				Assert.IsNotNull(transportMessage);

				messageId = transportMessage.MessageId;

				while (module.TransportMessage == null && DateTime.Now < timeout)
				{
					Thread.Sleep(5);
				}
			}

			Assert.IsNotNull(module.TransportMessage);
			Assert.True(messageId.Equals(module.TransportMessage.MessageId));
			Assert.True(messageType.Equals(module.TransportMessage.MessageType, StringComparison.OrdinalIgnoreCase));
		}
	}
}