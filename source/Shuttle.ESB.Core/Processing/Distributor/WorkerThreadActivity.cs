using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class WorkerThreadActivity : IThreadActivity
	{
		private readonly Guid identifier = Guid.NewGuid();
		private readonly IServiceBus bus;
		private readonly ThreadActivity threadActivity;

		private DateTime nextNotificationDate = DateTime.Now;

		private readonly ILog log;

		public WorkerThreadActivity(IServiceBus bus, ThreadActivity threadActivity)
		{
			Guard.AgainstNull(bus, "bus");
			Guard.AgainstNull(threadActivity, "threadActivity");

			this.bus = bus;
			this.threadActivity = threadActivity;

			log = Log.For(this);
		}

		public void Waiting(IThreadState state)
		{
			if (ShouldNotifyDistributor())
			{
				bus.Send(new WorkerThreadAvailableCommand
					{
						Identifier = identifier,
						InboxWorkQueueUri = bus.Configuration.Inbox.WorkQueue.Uri.ToString(),
						DateSent = DateTime.Now
					},
				         c => c.WithRecipient(bus.Configuration.Worker.DistributorControlInboxWorkQueue));

				if (log.IsVerboseEnabled)
				{
					log.Verbose(string.Format(ESBResources.DebugWorkerAvailable,
					                          identifier,
					                          bus.Configuration.Inbox.WorkQueue.Uri,
					                          bus.Configuration.Worker.DistributorControlInboxWorkQueue.Uri));
				}

				nextNotificationDate = DateTime.Now.AddSeconds(bus.Configuration.Worker.ThreadAvailableNotificationIntervalSeconds);
			}

			threadActivity.Waiting(state);
		}

		private bool ShouldNotifyDistributor()
		{
			return nextNotificationDate <= DateTime.Now;
		}

		public void Working()
		{
			nextNotificationDate = DateTime.Now;

			threadActivity.Working();
		}
	}
}