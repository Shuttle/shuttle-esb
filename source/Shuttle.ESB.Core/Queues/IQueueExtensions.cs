using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public static class IQueueExtensions
	{
		public static bool AttemptDrop(this IQueue queue)
		{
			var operation = queue as IDrop;

			if (operation == null)
			{
				return false;
			}

			operation.Drop();

			return true;
		}
		
		public static void Drop(this IQueue queue)
		{
			Guard.AgainstNull(queue, "queue");

			var operation = queue as IDrop;

			if (operation == null)
			{
				throw new InvalidOperationException(string.Format(ESBResources.NotImplementedOnQueue,
																  queue.GetType().FullName, "IDrop"));
			}

			operation.Drop();
		}
		
		public static bool AttemptPurge(this IQueue queue)
		{
			var operation = queue as IPurge;

			if (operation == null)
			{
				return false;
			}

			operation.Purge();

			return true;
		}
		
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