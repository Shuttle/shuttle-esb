using System;
using System.Threading;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class DistributorFixture : IntegrationFixture
	{
		private class WorkerModule : IModule, IPipelineObserver<OnAfterHandleMessage>
		{
			private readonly object padlock = new object();
			private readonly int _messageCount;
			private int _messagesHandled;
			private readonly ILog _log;

			public WorkerModule(int messageCount)
			{
				_messageCount = messageCount;

				_log = Log.For(this);
			}

			public void Initialize(IServiceBus bus)
			{
				bus.Events.PipelineCreated += PipelineCreated;
			}

			private void PipelineCreated(object sender, PipelineEventArgs e)
			{
				if (!e.Pipeline.GetType().FullName.Equals(typeof(InboxMessagePipeline).FullName, StringComparison.InvariantCultureIgnoreCase)
					&&
					!e.Pipeline.GetType().FullName.Equals(typeof(DeferredMessagePipeline).FullName, StringComparison.InvariantCultureIgnoreCase))
				{
					return;
				}

				e.Pipeline.RegisterObserver(this);
			}

			public void Execute(OnAfterHandleMessage pipelineEvent1)
			{
				_log.Information(string.Format("[OnAfterHandleMessage]"));

				lock (padlock)
				{
					_messagesHandled++;
				}
			}

			public bool AllMessagesHandled()
			{
				return _messagesHandled == _messageCount;
			}
		}

		private readonly ILog _log;

		public DistributorFixture()
		{
			_log = Log.For(this);
		}

		protected void TestDistributor(string queueUriFormat, bool isTransactional)
		{
			const int messageCount = 12;

			var module = new WorkerModule(messageCount);

			using (var distibutorBus = new ServiceBus(GetDistributorConfiguration(queueUriFormat, isTransactional)))
			using (var workerBus = new ServiceBus(GetWorkerConfiguration(queueUriFormat, isTransactional, module)))
			{
				for (var i = 0; i < messageCount; i++)
				{
					var command = new SimpleCommand
						{
							Name = Guid.NewGuid().ToString()
						};

					var workQueue = distibutorBus.Configuration.Inbox.WorkQueue;
					var message = distibutorBus.CreateTransportMessage(command, c => c.WithRecipient(workQueue));

					workQueue.Enqueue(message.MessageId, distibutorBus.Configuration.Serializer.Serialize(message));
				}

				distibutorBus.Start();
				workerBus.Start();

				var timeout = DateTime.Now.AddSeconds(15);
				var timedOut = false;

				_log.Information(string.Format("[start wait] : now = '{0}'", DateTime.Now));

				while (!module.AllMessagesHandled() && !timedOut)
				{
					Thread.Sleep(50);

					timedOut = timeout < DateTime.Now;
				}

				_log.Information(string.Format("[end wait] : now = '{0}' / timeout = '{1}' / timed out = '{2}'", DateTime.Now, timeout, timedOut));

				Assert.IsTrue(module.AllMessagesHandled(), "Not all messages were handled.");
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

		private static ServiceBusConfiguration GetWorkerConfiguration(string queueUriFormat, bool isTransactional, IModule module)
		{
			var configuration = DefaultConfiguration(isTransactional);

			configuration.Modules.Add(module);

			configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-worker-work")),
						ErrorQueue = configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-error")),
						DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
						ThreadCount = 1
					};

			configuration.Worker = new WorkerConfiguration(configuration.QueueManager.GetQueue(string.Format(queueUriFormat, "test-distributor-control")), 30);

			configuration.Inbox.WorkQueue.AttemptDrop();

			configuration.QueueManager.CreatePhysicalQueues(configuration);

			configuration.Inbox.WorkQueue.AttemptPurge();

			return configuration;
		}
	}
}