using System;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public static class PipelineEventExtensions
	{
		public static IServiceBus GetServiceBus(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<IServiceBus>();
		}

		public static IActiveState GetActiveState(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<IActiveState>(StateKeys.ActiveState);
		}

		public static IQueue GetWorkQueue(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<IQueue>(StateKeys.WorkQueue);
		}

		public static IQueue GetJournalQueue(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<IQueue>(StateKeys.JournalQueue);
		}

		public static IQueue GetErrorQueue(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<IQueue>(StateKeys.ErrorQueue);
		}

		public static int GetMaximumFailureCount(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<int>(StateKeys.MaximumFailureCount);
		}

		public static TimeSpan[] GetDurationToIgnoreOnFailure(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<TimeSpan[]>(StateKeys.DurationToIgnoreOnFailure);
		}

		public static void SetDestinationQueue(this PipelineEvent pipelineEvent, IQueue value)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.DestinationQueue, value);
		}

		public static IQueue GetDestinationQueue(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<IQueue>(StateKeys.DestinationQueue);
		}

		public static void SetTransportMessage(this PipelineEvent pipelineEvent, TransportMessage value)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.TransportMessage, value);
		}

		public static TransportMessage GetTransportMessage(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<TransportMessage>(StateKeys.TransportMessage);
		}

		public static void SetTransportMessageStream(this PipelineEvent pipelineEvent, Stream value)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.TransportMessageStream, value);
		}

		public static Stream GetTransportMessageStream(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<Stream>(StateKeys.TransportMessageStream);
		}

		public static byte[] GetMessageBytes(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<byte[]>(StateKeys.MessageBytes);
		}

		public static void SetMessageBytes(this PipelineEvent pipelineEvent, byte[] bytes)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.MessageBytes, bytes);
		}

		public static object GetMessage(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<object>(StateKeys.Message);
		}

		public static void SetMessage(this PipelineEvent pipelineEvent, object message)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.Message, message);
		}

		public static void SetAvailableWorker(this PipelineEvent pipelineEvent, AvailableWorker value)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.AvailableWorker, value);
		}

		public static AvailableWorker GetAvailableWorker(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<AvailableWorker>(StateKeys.AvailableWorker);
		}

		public static bool GetTransactionComplete(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<bool>(StateKeys.TransactionComplete);
		}

		public static void SetTransactionComplete(this PipelineEvent pipelineEvent)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.TransactionComplete, true);
		}

		public static bool GetWorking(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<bool>(StateKeys.Working);
		}

		public static void SetWorking(this PipelineEvent pipelineEvent)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.Working, true);
		}

		public static IServiceBusTransactionScope GetTransactionScope(this PipelineEvent pipelineEvent)
		{
            return pipelineEvent.Pipeline.State.Get<IServiceBusTransactionScope>(StateKeys.TransactionScope);
		}

		public static void SetTransactionScope(this PipelineEvent pipelineEvent, IServiceBusTransactionScope scope)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.TransactionScope, scope);
		}

		public static void SetMessageHandler(this PipelineEvent pipelineEvent, IMessageHandler handler)
		{
			pipelineEvent.Pipeline.State.Replace(StateKeys.MessageHandler, handler);
		}

		public static IMessageHandler GetMessageHandler(this PipelineEvent pipelineEvent)
		{
			return pipelineEvent.Pipeline.State.Get<IMessageHandler>(StateKeys.MessageHandler);
		}

        public static bool GetHasJournalQueue(this PipelineEvent pipelineEvent)
        {
            return pipelineEvent.Pipeline.State.Get<bool>(StateKeys.HasJournalQueue);
        }
	}
}