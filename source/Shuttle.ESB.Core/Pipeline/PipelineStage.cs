using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class PipelineStage
	{
		protected readonly List<PipelineEvent> events = new List<PipelineEvent>();

		public PipelineStage(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }

		public IEnumerable<PipelineEvent> Events
		{
			get { return new ReadOnlyCollection<PipelineEvent>(events); }
		}

		public PipelineStage WithEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new()
		{
			return WithEvent(new TPipelineEvent());
		}

		public PipelineStage WithEvent(PipelineEvent pipelineEvent)
		{
			Guard.AgainstNull(pipelineEvent, "pipelineEvent");

			events.Add(pipelineEvent);

			return this;
		}

		public RegisterEventBefore BeforeEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new()
		{
			var eventName = typeof(TPipelineEvent).FullName;
			var pipelineEvent = events.Find(e => e.Name.Equals(eventName));

			if (pipelineEvent == null)
			{
				throw new InvalidOperationException(string.Format(ESBResources.PipelineStageEventNotRegistered, Name, eventName));
			}

			return new RegisterEventBefore(events, pipelineEvent);
		}

		public RegisterEventAfter AfterEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new()
		{
			var eventName = typeof(TPipelineEvent).FullName;
			var pipelineEvent = events.Find(e => e.Name.Equals(eventName));

			if (pipelineEvent == null)
			{
				throw new InvalidOperationException(string.Format(ESBResources.PipelineStageEventNotRegistered, Name, eventName));
			}

			return new RegisterEventAfter(this, events, pipelineEvent);
		}

	}
}