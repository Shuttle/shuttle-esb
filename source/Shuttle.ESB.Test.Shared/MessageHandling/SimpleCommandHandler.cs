using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Shared
{
	public class SimpleCommandHandler : IMessageHandler<SimpleCommand>
	{
		private readonly ILog _log ;

		public SimpleCommandHandler()
		{
			_log = Log.For(this);
		}

		public void ProcessMessage(HandlerContext<SimpleCommand> context)
		{
			_log.Information("[executed]");
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}