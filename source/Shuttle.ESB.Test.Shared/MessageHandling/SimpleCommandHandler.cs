using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Shared
{
	public class SimpleCommandHandler : IMessageHandler<SimpleCommand>
	{
		public void ProcessMessage(HandlerContext<SimpleCommand> context)
		{
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}