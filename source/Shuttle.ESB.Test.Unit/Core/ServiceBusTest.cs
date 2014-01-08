using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Rhino.Mocks;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Unit.Core
{
	public class ServiceBusTest : UnitFixture
	{
		public TransportMessage TransportMessage { get; set; }

		[Test]
		public void Should_be_able_to_send_a_message_to_a_routed_queue()
		{
			var command = new SimpleCommand
											{
												Name = Guid.NewGuid().ToString()
											};

			var configuration = CreateMemoryConfiguration();

			var mockMessageRouteProvider = Mock<IMessageRouteProvider>();

			mockMessageRouteProvider.Stub(mock => mock.GetRouteUris(Arg<object>.Is.Anything)).Return(new List<string>
			{
			  configuration.Inbox.WorkQueue.Uri.ToString()
			});

			configuration.MessageRouteProvider = mockMessageRouteProvider;

			using (var bus = new ServiceBus(configuration))
			{
				bus.Configuration.Modules.Add(new MockModule(this));
	
				bus.Start();

				bus.Send(command);

				var waitTill = DateTime.Now.AddMilliseconds(15000);

				while (DateTime.Now < waitTill && TransportMessage == null)
				{
					Thread.Sleep(50);
				}

				Assert.IsNotNull(TransportMessage);
				Assert.AreEqual(command.Name, ((SimpleCommand)bus.Configuration.Serializer.Deserialize(typeof(SimpleCommand), new MemoryStream(TransportMessage.Message))).Name);
			}
		}

		[Test]
		public void Should_be_able_to_publish_an_event()
		{
			var mockSubscriptionManager = Mock<ISubscriptionManager>();

			const string SUBSCRIBER1_URI = "memory://./subscriber1";
			const string SUBSCRIBER2_URI = "memory://./subscriber2";

			var subscribers = new List<string>
			                  	{
			                  		SUBSCRIBER1_URI,
			                  		SUBSCRIBER2_URI
			                  	};

			mockSubscriptionManager.Stub(mock => mock.GetSubscribedUris(Arg<object>.Is.Anything)).Return(subscribers);

			var configuration = CreateMemoryConfiguration();

			configuration.SubscriptionManager = mockSubscriptionManager;

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

			Assert.NotNull(subscriber1.Dequeue());
			Assert.IsNull(subscriber1.Dequeue());
			Assert.NotNull(subscriber2.Dequeue());
			Assert.IsNull(subscriber2.Dequeue());
		}
	}
}