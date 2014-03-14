using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Shared.MessageHandling
{
	public class IdempotenceHandler : IMessageHandler<IdempotenceCommand>
	{
		private readonly IdempotenceCounter _counter;

		public IdempotenceHandler(IdempotenceCounter counter)
		{
			_counter = counter;
		}

		public void ProcessMessage(HandlerContext<IdempotenceCommand> context)
		{
			context.Bus.Send(new NoHandlerCommand());

			_counter.Processed();
		}

		public int ProcessedCount
		{
			get { return _counter.ProcessedCount; }
		}

		public bool IsReusable {
			get { return true; }
		}
	}
}