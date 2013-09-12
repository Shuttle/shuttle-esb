using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public static class IQueueExtensions
	{
		public static void Purge(this IQueue queue)
		{
			Guard.AgainstNull(queue, "queue");

			var operation = queue as IPurge;

			if (operation == null)
			{
				throw new InvalidOperationException(string.Format(ESBResources.NotImplementedOnQueue,
																  queue.GetType().FullName, "IPurge"));
			}

			operation.Purge();
		}
		
		public static int Count(this IQueue queue)
		{
			Guard.AgainstNull(queue, "queue");

			var operation = queue as ICount;

			if (operation == null)
			{
				throw new InvalidOperationException(string.Format(ESBResources.NotImplementedOnQueue,
																  queue.GetType().FullName, "ICount"));
			}

			return operation.Count;
		}
	}
}