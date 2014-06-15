using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ProcessorThreadPool : IProcessorThreadPool
	{
		private readonly string name;
		private readonly IProcessorFactory processorFactory;
		private readonly int threadCount;
		private readonly List<ProcessorThread> threads = new List<ProcessorThread>();
		private bool disposed;
		private bool started;
		private ILog _log;

		public ProcessorThreadPool(string name, int threadCount, IProcessorFactory processorFactory)
		{
			this.name = name;
			this.threadCount = threadCount;
			this.processorFactory = processorFactory;

			_log = Log.For(this);
		}

		public void Pause()
		{
			foreach (var thread in threads)
			{
				thread.Stop();
			}

			_log.Information(string.Format(ESBResources.ThreadPoolStatusChange, name, "paused"));
		}

		public void Resume()
		{
			foreach (var thread in threads)
			{
				thread.Start();
			}

			_log.Information(string.Format(ESBResources.ThreadPoolStatusChange, name, "resumed"));
		}

		public IProcessorThreadPool Start()
		{
			if (started)
			{
				return this;
			}

			if (threadCount < 1)
			{
				throw new ThreadCountZeroException();
			}

			StartThreads();

			started = true;

			_log.Information(string.Format(ESBResources.ThreadPoolStatusChange, name, "started"));

			return this;
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		private void StartThreads()
		{
			var i = 0;

			while (i++ < threadCount)
			{
				var thread = new ProcessorThread(processorFactory.Create());

				threads.Add(thread);

				thread.Start();
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			if (disposing)
			{
				foreach (var thread in threads)
				{
					thread.Deactivate();
				}

				foreach (var thread in threads)
				{
					thread.Stop();
				}
			}

			disposed = true;
		}
	}
}