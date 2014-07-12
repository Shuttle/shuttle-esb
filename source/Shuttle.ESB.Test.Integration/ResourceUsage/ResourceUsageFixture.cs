using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Integration
{
	public class ResourceUsageFixture : IntegrationFixture
	{
		public void TestResourceUsage(string workQueueUriFormat, string errorQueueUriFormat, bool isTransactional)
		{
			const int threadCount = 5;

			var configuration = GetConfiguration(workQueueUriFormat, errorQueueUriFormat, isTransactional, threadCount);

			var cpuCounter = new PerformanceCounterValue(new PerformanceCounter
				{
					CategoryName = "Processor",
					CounterName = "% Processor Time",
					InstanceName = "_Total"
				});

			var padlock = new object();
			var idleThreads = new List<int>();
			var startDate = DateTime.Now;
			var endDate = startDate.AddSeconds(10);
			var iteration = 0;
			float cpuUsageLimit;
			float cpuMaximumUsage = 0f;

			cpuCounter.NextValue();
			Thread.Sleep(1000);
			cpuUsageLimit = cpuCounter.NextValue() + 25F;

			using (var bus = new ServiceBus(configuration).Start())
			{
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

				while (DateTime.Now < endDate)
				{
					iteration++;

					for (var i = 0; i < 5; i++)
					{
						var message = bus.CreateTransportMessage(new SimpleCommand("[resource testing]"),
																c => c.WithRecipient(configuration.Inbox.WorkQueue));

						configuration.Inbox.WorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));
					}

					idleThreads.Clear();

					Console.WriteLine("[checking usage] : iteration = {0}", iteration);

					while (idleThreads.Count < threadCount)
					{
						var cpuUsage = cpuCounter.NextValue();

						if (cpuUsage > cpuMaximumUsage)
						{
							cpuMaximumUsage = cpuUsage;
						}

						Assert.IsTrue(cpuUsage < cpuUsageLimit,
									  string.Format("[EXCEEDED] : cpu usage = {0} / limit = {1}", cpuUsage, cpuUsageLimit));

						Thread.Sleep(25);
					}
				}

				Console.WriteLine("[done] : started = '{0}' / end = '{1}'", startDate, endDate);
				Console.WriteLine("[CPU] : maximum usage = {0} / cpu usage limit = {1}", cpuMaximumUsage, cpuUsageLimit);
			}
		}

		private static IServiceBusConfiguration GetConfiguration(string workQueueUriFormat, string errorQueueUriFormat, bool isTransactional, int threadCount)
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
						DurationToSleepWhenIdle = new[] { TimeSpan.FromSeconds(1) },
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