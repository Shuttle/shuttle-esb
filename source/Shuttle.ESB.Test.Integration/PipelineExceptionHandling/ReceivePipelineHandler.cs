using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration.Core
{
	public class ReceivePipelineHandler : IMessageHandler<ReceivePipelineCommand>
	{
		public void ProcessMessage(HandlerContext<ReceivePipelineCommand> context)
		{
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}