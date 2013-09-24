using System;
using System.Text;
using RequestResponse.Messages;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace RequestResponse.Server
{
	public class ServerMessageHandler : IMessageHandler<ReverseTextCommand>
	{
		private int received;

		public void ProcessMessage(HandlerContext<ReverseTextCommand> context)
		{
			received++;

			var sb = new StringBuilder();

			foreach (var c in context.Message.Text)
			{
				sb.Insert(0, c);
			}

			context.Bus.SendReply(
				new TextReversedEvent
					{
						ReversedText = sb.ToString()
					});

			Console.WriteLine("Message {0}: reversed text sent back to {1}.  Reversed text: {2}", received,
			                  context.TransportMessage.SenderInboxWorkQueueUri, sb);

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