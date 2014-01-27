using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.RabbitMQ;

namespace Shuttle.ESB.Test.Unit.RabbitMQ
{
	public class RabbitMQUriParserTests : UnitFixture
	{
		[Test]
		public void Should_be_able_to_parse_a_valid_uri()
		{
			var parser = new RabbitMQUriParser(new Uri("rabbitmq://./work-queue"));

			Assert.AreEqual("rabbitmq://localhost/work-queue", parser.Uri.ToString());
		}

		[Test]
		public void Should_be_able_to_parse_an_ip_address()
		{
			var parser = new RabbitMQUriParser(new Uri("rabbitmq://127.0.0.1/work-queue"));

			Assert.AreEqual("rabbitmq://127.0.0.1/work-queue", parser.Uri.ToString());
		}

		[Test]
		public void Should_throw_exception_when_trying_to_create_a_queue_with_incorrect_scheme()
		{
			Assert.Throws<InvalidSchemeException>(() => new RabbitMQUriParser(new Uri("sql://./work-queue")));
		}
	}
}