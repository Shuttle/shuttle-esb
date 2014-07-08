using System;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Shared
{
	public class ErrorCommandHandler : IMessageHandler<ErrorCommand>
	{
		public void ProcessMessage(HandlerContext<ErrorCommand> context)
		{
			throw new ApplicationException("[testing expection handling]");
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}