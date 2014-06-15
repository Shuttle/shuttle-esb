using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public enum PipelineStages
	{
		Entry = 0
	}

	public class Pipeline
	{
		protected readonly List<IObserver> _observers = new List<IObserver>();
		protected readonly Dictionary<string, List<IObserver>> _observedEvents = new Dictionary<string, List<IObserver>>();
		protected readonly List<PipelineStage> _stages = new List<PipelineStage>();

		private readonly OnPipelineStarting _onPipelineStarting = new OnPipelineStarting();
		private readonly OnAbortPipeline _onAbortPipeline = new OnAbortPipeline();
		private readonly OnPipelineException _onPipelineException = new OnPipelineException();

		private readonly string FirstChanceExceptionHandledByPipeline = ESBResources.VerboseFirstChanceExceptionHandledByPipeline;

		private readonly string ExecutingPipeline = ESBResources.VerboseExecutingPipeline;
		private readonly string EnteringPipelineStage = ESBResources.VerboseEnteringPipelineStage;
		private readonly string RaisingPipelineEvent = ESBResources.VerboseRaisingPipelineEvent;

		public Guid Id { get; private set; }
		public bool ExceptionHandled { get; internal set; }
		public Exception Exception { get; internal set; }
		public bool Aborted { get; internal set; }
		public string StageName { get; private set; }
		public PipelineEvent Event { get; private set; }

	    private readonly ILog _log;

		public Pipeline()
		{
			Id = Guid.NewGuid();
			State = new State<Pipeline>(this);
			_onAbortPipeline.Reset(this);
			_onPipelineException.Reset(this);

			var stage = new PipelineStage("__PipelineEntry");

			stage.WithEvent(_onPipelineStarting);

			_stages.Add(stage);

		    _log = Log.For(this);
		}

		public Pipeline RegisterObserver(IObserver observer)
		{
			List<IObserver> observerListForEvent;
			_observers.Add(observer);
			var observerInterfaces = observer.GetType().GetInterfaces();

			var implementedEvents = from i in observerInterfaces
			                        where i.IsAssignableTo(typeof (IPipelineObserver<>))
			                        select i;

			foreach (var @event in implementedEvents)
			{
				var pipelineEventName = @event.GetGenericArguments()[0].FullName;
				var pipelineEvent = (from observeEvent in _observedEvents
				                     where observeEvent.Key == pipelineEventName
				                     select observeEvent).SingleOrDefault();
				if (pipelineEvent.Key == null)
				{
					_observedEvents.Add(pipelineEventName, new List<IObserver>());
				}

				_observedEvents.TryGetValue(pipelineEventName, out observerListForEvent);
				observerListForEvent.Add(observer);
			}
			return this;
		}

		public State<Pipeline> State { get; private set; }

		public void Abort()
		{
			Aborted = true;
		}

		public void MarkExceptionHandled()
		{
			ExceptionHandled = true;
		}

		public virtual bool Execute()
		{
			var result = true;

			Aborted = false;
			ExceptionHandled = false;
			Exception = null;

			_log.Verbose(string.Format(ExecutingPipeline, GetType().FullName));

			foreach (var stage in _stages)
			{
				StageName = stage.Name;

				_log.Verbose(string.Format(EnteringPipelineStage, StageName));

				foreach (var @event in stage.Events)
				{
					try
					{
						Event = @event;

						RaiseEvent(@event.Reset(this));

						if (Aborted)
						{
							result = false;

							RaiseEvent(_onAbortPipeline);

							break;
						}
					}
					catch (Exception ex)
					{
						result = false;

						Exception = ex.TrimLeading<TargetInvocationException>();

						RaiseEvent(_onPipelineException, true);

						if (!ExceptionHandled)
						{
							_log.Fatal(string.Format(ESBResources.UnhandledPipelineException, @event.Name, ex.AllMessages()));

							throw;
						}

						_log.Verbose(string.Format(FirstChanceExceptionHandledByPipeline, ex.Message));

						if (Aborted)
						{
							RaiseEvent(_onAbortPipeline);

							break;
						}
					}
				}

				if (Aborted)
				{
					break;
				}
			}

			return result;
		}

	    private void RaiseEvent(OnAbortPipeline @event)
	    {
            RaiseEvent(@event, true);
	    }

	    private void RaiseEvent(PipelineEvent @event, bool ignoreAbort = false)
		{
			var observersForEvent = (from e in _observedEvents
			                         where e.Key == @event.GetType().FullName
			                         select e.Value).SingleOrDefault();

			if (observersForEvent == null || observersForEvent.Count == 0)
			{
				return;
			}

			foreach (var observer in observersForEvent)
			{
				_log.Verbose(string.Format(RaisingPipelineEvent, @event.Name, StageName, observer.GetType().FullName));

				observer.GetType().InvokeMember("Execute",
				                                BindingFlags.FlattenHierarchy | BindingFlags.Instance |
				                                BindingFlags.InvokeMethod | BindingFlags.Public, null,
				                                observer,
				                                new[] {@event});

				if (Aborted && !ignoreAbort)
				{
					return;
				}
			}
		}

		public PipelineStage RegisterStage(string name)
		{
			var stage = new PipelineStage(name);

			_stages.Add(stage);

			return stage;
		}

		public PipelineStage GetStage(string name)
		{
			var result = _stages.Find(stage => stage.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

			Guard.Against<IndexOutOfRangeException>(result == null,
			                                        string.Format(ESBResources.PipelineStageNotFound, name));

			return result;
		}

		public PipelineStage GetStage(PipelineStages stage)
		{
			return GetStage("__PipelineEntry");
		}
	}
}