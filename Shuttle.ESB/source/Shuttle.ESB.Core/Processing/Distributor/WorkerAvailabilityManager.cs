using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class WorkerAvailabilityManager : IWorkerAvailabilityManager
    {
        private static readonly object padlock = new object();

        private List<AvailableWorker> availableWorkers = new List<AvailableWorker>();

		private readonly ILog log;

		public WorkerAvailabilityManager()
		{
			log = Log.For(this);
		}

        public AvailableWorker GetAvailableWorker()
        {
            lock (padlock)
            {
                if (availableWorkers.Count == 0)
                {
                    return null;
                }

                var result = availableWorkers[0];

                availableWorkers.RemoveAt(0);

                return result;
            }
        }

        public void WorkerAvailable(WorkerThreadAvailableCommand message)
        {
            lock (padlock)
            {
                if (Contains(message.Identifier))
                {
                    return;
                }

                availableWorkers.Add(new AvailableWorker(message));
            }

            if (log.IsTraceEnabled)
            {
                log.Trace(string.Format("AvailableWorker: {0}", message.InboxWorkQueueUri));
            }
        }

        private bool Contains(Guid identifier)
        {
            return availableWorkers.Find(availableWorker => availableWorker.Identifier.Equals(identifier)) != null;
        }

        public void ReturnAvailableWorker(AvailableWorker availableWorker)
        {
			if (availableWorker == null)
			{
				return;
			}

        	lock (padlock)
            {
                availableWorkers.Add(availableWorker);
            }
        }

        public void WorkerStarted(WorkerStartedEvent message)
        {
            lock (padlock)
            {
                var result = new List<AvailableWorker>();

                foreach (var availableWorker in availableWorkers)
                {
                    if (
                        !(availableWorker.InboxWorkQueueUri.Equals(message.InboxWorkQueueUri,
                                                          StringComparison.InvariantCultureIgnoreCase) &&
                          availableWorker.WorkerSendDate < message.DateStarted))
                    {
                        result.Add(availableWorker);
                    }
                }

                availableWorkers = result;
            }
        }
    }
}