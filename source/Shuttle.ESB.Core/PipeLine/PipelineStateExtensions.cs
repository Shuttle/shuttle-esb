using System;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public static class PipelineStateExtensions
	{
		public static IServiceBus GetServiceBus(this State<ObservablePipeline> state)
		{
			return state.Get<IServiceBus>();
		}

		public static IActiveState GetActiveState(this State<ObservablePipeline> state)
		{
			return state.Get<IActiveState>(StateKeys.ActiveState);
		}

		public static IQueue GetWorkQueue(this State<ObservablePipeline> state)
		{
			return state.Get<IQueue>(StateKeys.WorkQueue);
		}

		public static IQueue GetDeferredQueue(this State<ObservablePipeline> state)
		{
			return state.Get<IQueue>(StateKeys.DeferredQueue);
		}

		public static IQueue GetErrorQueue(this State<ObservablePipeline> state)
		{
			return state.Get<IQueue>(StateKeys.ErrorQueue);
		}

		public static int GetMaximumFailureCount(this State<ObservablePipeline> state)
		{
			return state.Get<int>(StateKeys.MaximumFailureCount);
		}

		public static TimeSpan[] GetDurationToIgnoreOnFailure(this State<ObservablePipeline> state)
		{
			return state.Get<TimeSpan[]>(StateKeys.DurationToIgnoreOnFailure);
		}

		public static void SetDestinationQueue(this State<ObservablePipeline> state, IQueue value)
		{
			state.Replace(StateKeys.DestinationQueue, value);
		}

		public static IQueue GetDestinationQueue(this State<ObservablePipeline> state)
		{
			return state.Get<IQueue>(StateKeys.DestinationQueue);
		}

		public static void SetTransportMessage(this State<ObservablePipeline> state, TransportMessage value)
		{
			state.Replace(StateKeys.TransportMessage, value);
		}

		public static TransportMessage GetTransportMessage(this State<ObservablePipeline> state)
		{
			return state.Get<TransportMessage>(StateKeys.TransportMessage);
		}

		public static void SetTransportMessageStream(this State<ObservablePipeline> state, Stream value)
		{
			state.Replace(StateKeys.TransportMessageStream, value);
		}

		public static Stream GetTransportMessageStream(this State<ObservablePipeline> state)
		{
			return state.Get<Stream>(StateKeys.TransportMessageStream);
		}

		public static byte[] GetMessageBytes(this State<ObservablePipeline> state)
		{
			return state.Get<byte[]>(StateKeys.MessageBytes);
		}

		public static void SetMessageBytes(this State<ObservablePipeline> state, byte[] bytes)
		{
			state.Replace(StateKeys.MessageBytes, bytes);
		}

		public static DateTime GetIgnoreTillDate(this State<ObservablePipeline> state)
		{
			return state.Get<DateTime>(StateKeys.IgnoreTillDate);
		}

		public static object GetMessage(this State<ObservablePipeline> state)
		{
			return state.Get<object>(StateKeys.Message);
		}

		public static void SetMessage(this State<ObservablePipeline> state, object message)
		{
			state.Replace(StateKeys.Message, message);
		}

		public static void SetAvailableWorker(this State<ObservablePipeline> state, AvailableWorker value)
		{
			state.Replace(StateKeys.AvailableWorker, value);
		}

		public static AvailableWorker GetAvailableWorker(this State<ObservablePipeline> state)
		{
			return state.Get<AvailableWorker>(StateKeys.AvailableWorker);
		}

		public static bool GetTransactionComplete(this State<ObservablePipeline> state)
		{
			return state.Get<bool>(StateKeys.TransactionComplete);
		}

		public static void SetTransactionComplete(this State<ObservablePipeline> state)
		{
			state.Replace(StateKeys.TransactionComplete, true);
		}

		public static void SetWorking(this State<ObservablePipeline> state)
		{
			state.Replace(StateKeys.Working, true);
		}

		public static void ResetWorking(this State<ObservablePipeline> state)
		{
			state.Replace(StateKeys.Working, false);
		}

		public static IServiceBusTransactionScope GetTransactionScope(this State<ObservablePipeline> state)
		{
			return state.Get<IServiceBusTransactionScope>(StateKeys.TransactionScope);
		}

		public static void SetTransactionScope(this State<ObservablePipeline> state, IServiceBusTransactionScope scope)
		{
			state.Replace(StateKeys.TransactionScope, scope);
		}

		public static void SetMessageHandler(this State<ObservablePipeline> state, IMessageHandler handler)
		{
			state.Replace(StateKeys.MessageHandler, handler);
		}

		public static IMessageHandler GetMessageHandler(this State<ObservablePipeline> state)
		{
			return state.Get<IMessageHandler>(StateKeys.MessageHandler);
		}

		public static Guid GetCheckpointMessageId(this State<ObservablePipeline> state)
		{
			return state.Get<Guid>(StateKeys.CheckpointMessageId);
		}

		public static void SetCheckpointMessageId(this State<ObservablePipeline> state, Guid checkpointMessageId)
		{
			state.Replace(StateKeys.CheckpointMessageId, checkpointMessageId);
		}

		public static DateTime GetNextDeferredProcessDate(this State<ObservablePipeline> state)
		{
			return state.Get<DateTime>(StateKeys.NextDeferredProcessDate);
		}

		public static void SetNextDeferredProcessDate(this State<ObservablePipeline> state, DateTime nextDeferredProcessDate)
		{
			state.Replace(StateKeys.NextDeferredProcessDate, nextDeferredProcessDate);
		}

		public static bool GetDeferredMessageReturned(this State<ObservablePipeline> state)
		{
			return state.Get<bool>(StateKeys.DeferredMessageReturned);
		}

		public static void SetDeferredMessageReturned(this State<ObservablePipeline> state, bool deferredMessageReturned)
		{
			state.Replace(StateKeys.DeferredMessageReturned, deferredMessageReturned);
		}

		public static void SetWorkQueue(this State<ObservablePipeline> state, IQueue queue)
		{
			state.Add(StateKeys.WorkQueue, queue);
		}

		public static void SetDeferredQueue(this State<ObservablePipeline> state, IQueue queue)
		{
			state.Add(StateKeys.DeferredQueue, queue);
		}

		public static void SetErrorQueue(this State<ObservablePipeline> state, IQueue queue)
		{
			state.Add(StateKeys.ErrorQueue, queue);
		}

		public static void SetIgnoreTillDate(this State<ObservablePipeline> state, DateTime date)
		{
			state.Add(StateKeys.IgnoreTillDate, date);
		}

		public static void SetMaximumFailureCount(this State<ObservablePipeline> state, int count)
		{
			state.Add(StateKeys.MaximumFailureCount, count);
		}

		public static void SetDurationToIgnoreOnFailure(this State<ObservablePipeline> state, TimeSpan[] timeSpans)
		{
			state.Add(StateKeys.DurationToIgnoreOnFailure, timeSpans);
		}

		public static void SetActiveState(this State<ObservablePipeline> state, IActiveState activeState)
		{
			state.Add(StateKeys.ActiveState, activeState);
		}
	}
}