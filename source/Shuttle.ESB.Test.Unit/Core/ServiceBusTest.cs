using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Moq;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Unit.Core
{
	public class ServiceBusTest : UnitFixture
	{
		[Test]
		public void Should_be_able_to_send_a_message_to_a_routed_queue()
		{
			var command = new SimpleCommand
											{
												Name = Guid.NewGuid().ToString()
											};

			var configuration = CreateMemoryConfiguration();

			var module = new ServiceBusRoutingModule();

			configuration.Modules.Add(module);

			var mockMessageRouteProvider = new Mock<IMessageRouteProvider>();

			mockMessageRouteProvider.Setup(mock => mock.GetRouteUris(It.IsAny<string>())).Returns(new List<string>
			{
			  configuration.Inbox.WorkQueue.Uri.ToString()
			});

			configuration.MessageRouteProvider = mockMessageRouteProvider.Object;

			using (var bus = new ServiceBus(configuration))
			{
				bus.Start();

				bus.Send(command);

				var timeout = DateTime.Now.AddMilliseconds(5000);

				while (DateTime.Now < timeout && module.SimpleCommand == null)
				{
					Thread.Sleep(50);
				}

				Assert.IsNotNull(module.SimpleCommand);
				Assert.AreEqual(command.Name, module.SimpleCommand.Name);
			}
		}

		[Test]
		public void Should_be_able_to_publish_an_event()
		{
			var mockSubscriptionManager = new Mock<ISubscriptionManager>();

			const string SUBSCRIBER1_URI = "memory://./subscriber1";
			const string SUBSCRIBER2_URI = "memory://./subscriber2";

			var subscribers = new List<string>
			                  	{
			                  		SUBSCRIBER1_URI,
			                  		SUBSCRIBER2_URI
			                  	};

			mockSubscriptionManager.Setup(mock => mock.GetSubscribedUris(It.IsAny<object>())).Returns(subscribers);

			var configuration = CreateMemoryConfiguration();

			configuration.SubscriptionManager = mockSubscriptionManager.Object;

			var subscriber1 = configuration.QueueManager.CreateQueue(SUBSCRIBER1_URI);
			var subscriber2 = configuration.QueueManager.CreateQueue(SUBSCRIBER2_URI); ;

			using (var bus = new ServiceBus(configuration).Start())
			{
				const string EVENT_NAME = "test::event";

				bus.Publish(new SimpleEvent
											{
												EventName = EVENT_NAME
											});
			}

			Assert.NotNull(subscriber1.GetMessage());
			Assert.IsNull(subscriber1.GetMessage());
			Assert.NotNull(subscriber2.GetMessage());
			Assert.IsNull(subscriber2.GetMessage());
		}
	}
}