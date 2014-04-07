using System;
using System.Threading;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration.Core
{
	public class PipelineExceptionFixture : IntegrationFixture
	{
		protected void TestExceptionHandling(string workQueueUriFormat, string errorQueueUriFormat)
		{
			var configuration = DefaultConfiguration(true);

			var inboxWorkQueue = configuration.QueueManager.GetQueue(string.Format(workQueueUriFormat, "test-inbox-work"));
			var inboxErrorQueue = configuration.QueueManager.GetQueue(string.Format(errorQueueUriFormat, "test-error"));

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = inboxWorkQueue,
						ErrorQueue = inboxErrorQueue,
						DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
						DurationToIgnoreOnFailure = new[] { TimeSpan.FromMilliseconds(5) },
						MaximumFailureCount = 100,
						ThreadCount = 1
					};


			inboxWorkQueue.Drop();
			inboxErrorQueue.Drop();

			configuration.QueueManager.CreatePhysicalQueues(configuration);

			var module = new ReceivePipelineExceptionModule(inboxWorkQueue);

			configuration.Modules.Add(module);

			using (var bus = new ServiceBus(configuration))
			{
				var message = bus.CreateTransportMessage(new ReceivePipelineCommand());

				inboxWorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));

				Assert.IsFalse(inboxWorkQueue.IsEmpty());

				bus.Start();

				while (module.ShouldWait())
				{
					Thread.Sleep(10);
				}
			}
		}
	}
}