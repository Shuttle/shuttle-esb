using System;
using PublishSubscribe.Messages;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace PublishSubscribe.Client
{
    public class ClientHandler : IMessageHandler<WorkDoneEvent>
	{
		public void ProcessMessage(HandlerContext<WorkDoneEvent> context)
		{
			ColoredConsole.WriteLine(ConsoleColor.Blue, context.Message.Comment);
		}

        public bool IsReusable
		{
			get { return true; }
		}
	}
}