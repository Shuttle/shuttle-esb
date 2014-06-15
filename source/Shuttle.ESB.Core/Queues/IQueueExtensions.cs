using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public static class IQueueExtensions
	{
		public static bool AttemptCreate(this IQueue queue)
		{
			var operation = queue as ICreateQueue;

			if (operation == null)
			{
				return false;
			}

			operation.Create();

			return true;
		}
		
		public static void Create(this IQueue queue)
		{
			Guard.AgainstNull(queue, "queue");

			var operation = queue as ICreateQueue;

			if (operation == null)
			{
				throw new InvalidOperationException(string.Format(ESBResources.NotImplementedOnQueue,
																  queue.GetType().FullName, "ICreateQueue"));
			}

			operation.Create();
		}
		
		public static bool AttemptDrop(this IQueue queue)
		{
			var operation = queue as IDropQueue;

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

			var operation = queue as IDropQueue;

			if (operation == null)
			{
				throw new InvalidOperationException(string.Format(ESBResources.NotImplementedOnQueue,
																  queue.GetType().FullName, "IDropQueue"));
			}

			operation.Drop();
		}
		
		public static bool AttemptPurge(this IQueue queue)
		{
			var operation = queue as IPurgeQueue;

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

			var operation = queue as IPurgeQueue;

			if (operation == null)
			{
				throw new InvalidOperationException(string.Format(ESBResources.NotImplementedOnQueue,
																  queue.GetType().FullName, "IPurgeQueue"));
			}

			operation.Purge();
		}
	}
}