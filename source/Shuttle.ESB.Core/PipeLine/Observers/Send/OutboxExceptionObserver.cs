using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class OutboxExceptionObserver : IPipelineObserver<OnPipelineException>
	{
		public void Execute(OnPipelineException pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var bus = state.GetServiceBus();

			bus.Events.OnBeforePipelineExceptionHandled(this, new PipelineExceptionEventArgs(pipelineEvent.Pipeline));

			try
			{
				if (pipelineEvent.Pipeline.ExceptionHandled)
				{
					return;
				}

				try
				{
					if (!pipelineEvent.Pipeline.StageName.Equals("Send"))
					{
						return;
					}

					var transportMessage = state.GetTransportMessage();

					if (transportMessage == null)
					{
						return;
					}

				    var action = bus.Configuration.Policy.EvaluateOutboxFailure(pipelineEvent);

					transportMessage.RegisterFailure(pipelineEvent.Pipeline.Exception.CompactMessages(), action.TimeSpanToIgnoreRetriedMessage);

					if (action.Retry)
					{
						state.GetWorkQueue().Enqueue(
							transportMessage.MessageId,
							state.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
					}
					else
					{
						state.GetErrorQueue().Enqueue(
							transportMessage.MessageId,
							state.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
					}

					state.SetTransactionComplete();
				}
				finally
				{
					pipelineEvent.Pipeline.MarkExceptionHandled();
					bus.Events.OnAfterPipelineExceptionHandled(this, new PipelineExceptionEventArgs(pipelineEvent.Pipeline));
				}
			}
			finally
			{
				pipelineEvent.Pipeline.Abort();
			}
		}
	}
}