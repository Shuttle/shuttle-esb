using System;
using System.Threading;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration.Core
{
	public class PipelineExceptionHandlingTest : IntegrationFixture
	{
		[Test]
		public void Should_be_able_to_roll_back_any_database_and_queue_changes_when_an_exception_occurs_in_the_receive_pipeline()
		{
			var configuration = DefaultConfiguration(true);

			var inboxWorkQueue = QueueManager.Instance.GetQueue("msmq://./test-inbox-work");
			var inboxJournalQueue = QueueManager.Instance.GetQueue("msmq://./test-inbox-journal");
			var inboxErrorQueue = QueueManager.Instance.GetQueue("msmq://./test-error");

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = inboxWorkQueue,
						JournalQueue = inboxJournalQueue,
						ErrorQueue = inboxErrorQueue,
						DurationToSleepWhenIdle = new[] {TimeSpan.FromMilliseconds(5)},
						DurationToIgnoreOnFailure = new[] {TimeSpan.FromMilliseconds(5)},
						MaximumFailureCount = 100,
						ThreadCount = 1
					};


			inboxWorkQueue.Drop();
			inboxJournalQueue.Drop();
			inboxErrorQueue.Drop();
			
			QueueManager.Instance.CreatePhysicalQueues(configuration, QueueCreationType.All);

			var module = new ReceivePipelineExceptionModule(inboxWorkQueue);

			configuration.Modules.Add(module);

			using (var bus = new ServiceBus(configuration))
			{
				var message = bus.CreateTransportMessage(new ReceivePipelineCommand());

				inboxWorkQueue.Enqueue(message.MessageId, configuration.Serializer.Serialize(message));

				Assert.AreEqual(1, inboxWorkQueue.Count());

				bus.Start();

				while (module.ShouldWait())
				{
					Thread.Sleep(10);
				}
			}
		}
	}
}