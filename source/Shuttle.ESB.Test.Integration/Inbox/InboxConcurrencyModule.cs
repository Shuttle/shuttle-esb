using System;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
	public class InboxConcurrencyModule :
		IModule,
		IPipelineObserver<OnAfterGetMessage>
	{
		private readonly object _padlock = new object();
		private readonly List<DateTime> _datesAfterGetMessage = new List<DateTime>();
		private DateTime _firstDateAfterGetMessage = DateTime.MinValue;

		public void Initialize(IServiceBus bus)
		{
			bus.Events.PipelineCreated += PipelineCreated;
		}

		private void PipelineCreated(object sender, PipelineEventArgs e)
		{
			if (!e.Pipeline.GetType().FullName.Equals(typeof(InboxMessagePipeline).FullName, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			e.Pipeline.RegisterObserver(this);
		}

		public void Execute(OnAfterGetMessage pipelineEvent)
		{
			lock (_padlock)
			{
				var dateTime = DateTime.Now;

				if (_firstDateAfterGetMessage == DateTime.MinValue)
				{
					_firstDateAfterGetMessage = DateTime.Now;

					Console.WriteLine("Offset date: {0}", _firstDateAfterGetMessage.ToString("yyyy-MM-dd HH:mm:ss.fff"));
				}

				_datesAfterGetMessage.Add(dateTime);

				Console.WriteLine("Dequeued date: {0}", dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
			}
		}

		public int OnAfterGetMessageCount
		{
			get { return _datesAfterGetMessage.Count; }
		}

		public bool AllMessagesReceivedWithinTimespan(int msToComplete)
		{
			return _datesAfterGetMessage.All(dateTime => dateTime.Subtract(_firstDateAfterGetMessage) <= TimeSpan.FromMilliseconds(msToComplete));
		}
	}
}