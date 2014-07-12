using System;
using System.Diagnostics;
using System.Threading;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared.Mocks;
using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class ResourceUsageFixture : IntegrationFixture
	{
		public void TestResourceUsage(string workQueueUriFormat, string errorQueueUriFormat, bool isTransactional)
		{
			var configuration = GetConfiguration(workQueueUriFormat, errorQueueUriFormat, isTransactional);

			var cpu = new PerformanceCounter
				{
					CategoryName = "Processor",
					CounterName = "% Processor Time",
					InstanceName = "_Total"
				};

			var startDate = DateTime.Now;
			var endDate = startDate.AddSeconds(10);
			var limit = 25;

			using (var bus = new ServiceBus(configuration).Start())
			{
				while (DateTime.Now < endDate)
				{
					for (var i = 0; i < 5; i++)
					{
						var warmup = bus.CreateTransportMessage(new SimpleCommand("warmup"),
																c => c.WithRecipient(configuration.Inbox.WorkQueue));

						configuration.Inbox.WorkQueue.Enqueue(warmup.MessageId, configuration.Serializer.Serialize(warmup));
					}

					Thread.Sleep(TimeSpan.FromMilliseconds(250));

					var cpuUsage = cpu.NextValue();

					Console.WriteLine("[counter] : cpu = {0} / limit = '{1}'", cpuUsage, limit);
					Assert.IsTrue(cpuUsage < limit);
				}

				Console.WriteLine("[done] : started = '{0}' / end = '{1}'", startDate, endDate);
			}
		}

		private static IServiceBusConfiguration GetConfiguration(string workQueueUriFormat, string errorQueueUriFormat, bool isTransactional)
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
					DurationToSleepWhenIdle = new[] { TimeSpan.FromSeconds(5) },
					ThreadCount = 1
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