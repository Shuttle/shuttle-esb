using System;
using System.Collections.Generic;
using System.Threading;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public class TaskQueue : IDisposable, IActiveState
	{
		private readonly object padlock = new object();
		private readonly Queue<QueuedTask> tasks = new Queue<QueuedTask>();
		private readonly Thread thread;
		private volatile bool active;

		public TaskQueue()
		{
			Log.Debug("Starting TaskQueue.");

			active = true;

			thread = new Thread(ProcessQueuedActions);

			thread.Start();

			while (!thread.IsAlive)
			{
			}

			Log.Debug("TaskQueue Started.");
		}

		public void QueueTask(string name, Action action)
		{
			lock (padlock)
			{
				Log.Information(string.Format(ManagementResources.TaskQueued, name));

				tasks.Enqueue(new QueuedTask(name, action));
			}
		}

		private void ProcessQueuedActions()
		{
			while (active)
			{
				QueuedTask task = null;

				lock (padlock)
				{
					if (tasks.Count > 0)
					{
						task = tasks.Dequeue();
					}
				}

				if (task != null)
				{
					Log.Information(string.Format(ManagementResources.RunningTask, task.Name));

					ExceptionWrapper(task.Action);

					Log.Information(string.Format(ManagementResources.TaskCompleted, task.Name));
				}

				if (task == null)
				{
					ThreadSleep.While(250, this);
				}
			}
		}

		private static void ExceptionWrapper(Action action)
		{
			try
			{
				action.Invoke();
			}
			catch (Exception exception)
			{
				Log.Error(exception.CompactMessages());
			}
		}

		internal class QueuedTask
		{
			public QueuedTask(string name, Action action)
			{
				Name = name;
				Action = action;
			}

			public string Name { get; private set; }
			public Action Action { get; private set; }
		}

		public void Dispose()
		{
			Log.Debug("Deactivating TaskQueue.");

			active = false;

			Log.Debug("Joinging UI thread.");

			thread.Join(TimeSpan.FromSeconds(5));

			Log.Debug("TaskQueue disposing.");
		}

		public bool Active
		{
			get { return active; }
		}
	}
}