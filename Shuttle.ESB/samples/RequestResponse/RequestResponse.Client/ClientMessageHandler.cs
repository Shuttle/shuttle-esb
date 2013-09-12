using System;
using RequestResponse.Messages;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace RequestResponse.Client
{
    public class ClientMessageHandler : IMessageHandler<TextReversedEvent>
    {
		public void ProcessMessage(HandlerContext<TextReversedEvent> context)
    	{
			Console.WriteLine("Text has been reversed: {0}", context.Message.ReversedText);

			foreach (var header in context.TransportMessage.Headers)
			{
				ColoredConsole.WriteLine(ConsoleColor.Cyan, "<header> '{0}' : '{1}'", header.Key, header.Value);
			}
		}

    	public bool IsReusable
    	{
    		get { return true; }
    	}
    }
}
