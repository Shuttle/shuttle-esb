using System;
using log4net;
using RequestResponse.Messages;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Log4Net;
using Shuttle.ESB.Core;

namespace RequestResponse.Client
{
	internal class Program
	{
		private static void Main()
		{
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Program))));

			var bus = ServiceBus
				.Create()
				.AddCompressionAlgorithm(new GZipCompressionAlgorithm())
				.AddEnryptionAlgorithm(new TripleDesEncryptionAlgorithm())
				.Start();

			Console.WriteLine("Client bus started.  Press CTRL+C to stop.");
			Console.WriteLine();
			Console.WriteLine("Press enter to send a message with random text to the server for reversal.");
			Console.WriteLine();

			while (true)
			{
				Console.ReadLine();

				var command = new ReverseTextCommand
							  {
								  Text = Guid.NewGuid().ToString().Substring(0, 5)
							  };

				bus.OutgoingCorrelationId = "correlation-id";
				bus.OutgoingHeaders.Add(new TransportHeader { Key = "header1", Value = "value1" });
				bus.OutgoingHeaders.Add(new TransportHeader { Key = "header2", Value = "value2" });

				Console.WriteLine("Message (id: {0}) sent.  Text: {1}", bus.Send(command).MessageId, command.Text);
			}
		}
	}
}