using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Msmq;
using Shuttle.ESB.RabbitMq;

namespace Shuttle.ESB.Test.Unit.RabbitMq
{
	public class RabbitMqQueueTest : UnitFixture
	{
		private RabbitMqQueueFactory _factory;

		protected override void TestSetUp()
		{
			base.TestSetUp();
			_factory = new RabbitMqQueueFactory();			
		}

		protected override void TestTearDown()
		{
			_factory.Dispose();
		}

		[Test]
		public void Should_be_able_to_create_a_new_queue_from_a_given_uri()
		{
			Assert.AreEqual(string.Format("rabbitmq://{0}/inputqueue", Environment.MachineName.ToLower()),
				_factory.Create(new Uri("rabbitmq://localhost/inputqueue")).Uri.ToString());
		}

		[Test]
		public void Should_be_able_to_create_a_new_queue_using_an_ip_address()
		{
			Assert.AreEqual("rabbitmq://127.0.0.1/inputqueue",
				_factory.Create(new Uri("rabbitmq://127.0.0.1/inputqueue")).Uri.ToString());
		}

		[Test]
		public void Should_throw_exception_when_trying_to_create_a_queue_with_incorrect_scheme()
		{
			Assert.Throws<InvalidSchemeException>(() => _factory.Create(new Uri("sql://127.0.0.1/inputqueue")));
		}
	}
}
