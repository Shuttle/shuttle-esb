using System;
using PublishSubscribe.Messages;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.ESB.SqlServer;

namespace PublishSubscribe.Client
{
	internal class Program
	{
		private static void Main()
		{
			Log.Assign(new ConsoleLog(typeof(Program)) { LogLevel = LogLevel.Trace });

			new ConnectionStringService().Approve();

		    var subscriptionManager = SubscriptionManager.Default();

            subscriptionManager.Subscribe(new[] { typeof(WorkDoneEvent).FullName });

		    var bus = ServiceBus
				.Create()
				.SubscriptionManager(subscriptionManager)
				.Start();

			Console.WriteLine();
			ColoredConsole.WriteLine(ConsoleColor.Green, "Client bus started.  Press CTRL+C to stop.");
			Console.WriteLine();
			ColoredConsole.WriteLine(ConsoleColor.Green, "Press enter to publish an OrderCompleted event.");
			Console.WriteLine();

			while (true)
			{
				Console.ReadLine();

				var message = new OrderCompletedEvent
								  {
									  OrderId = Guid.NewGuid()
								  };

				bus.Publish(message);

				Console.WriteLine("Published OrderCompleted Id: {0}", message.OrderId);
			}
		}
	}
}