using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class OutboxExceptionObserver : IPipelineObserver<OnPipelineException>
	{
		public void Execute(OnPipelineException pipelineEvent)
		{
			var bus = pipelineEvent.GetServiceBus();

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

					var transportMessage = pipelineEvent.GetTransportMessage();

					if (transportMessage == null)
					{
						return;
					}

				    var action = bus.Configuration.Policy.EvaluateOutboxFailure(pipelineEvent);

					transportMessage.RegisterFailure(pipelineEvent.Pipeline.Exception.CompactMessages(), action.TimeSpanToIgnoreRetriedMessage);

					if (action.Retry)
					{
						pipelineEvent.GetWorkQueue().Enqueue(
							transportMessage.MessageId,
							pipelineEvent.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
					}
					else
					{
						pipelineEvent.GetErrorQueue().Enqueue(
							transportMessage.MessageId,
							pipelineEvent.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
					}

					pipelineEvent.SetTransactionComplete();
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