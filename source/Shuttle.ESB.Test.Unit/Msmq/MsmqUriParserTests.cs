using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Msmq;

namespace Shuttle.ESB.Test.Unit.Msmq
{
	public class MsmqUriParserTests : UnitFixture
	{
		[Test]
		public void Should_be_able_to_parse_a_valid_uri()
		{
			var parser = new MsmqUriParser(new Uri("msmq://./work-queue"));

			Assert.AreEqual(string.Format("msmq://{0}/work-queue", Environment.MachineName.ToLower()), parser.Uri.ToString());
		}

		[Test]
		public void Should_be_able_to_parse_an_ip_address()
		{
			var parser = new MsmqUriParser(new Uri("msmq://127.0.0.1/work-queue"));

			Assert.AreEqual("msmq://127.0.0.1/work-queue", parser.Uri.ToString());
		}

		[Test]
		public void Should_throw_exception_when_trying_to_create_a_queue_with_incorrect_scheme()
		{
			Assert.Throws<InvalidSchemeException>(() => new MsmqUriParser(new Uri("sql://./work-queue")));
		}

		[Test]
		public void Should_be_able_to_create_with_default_parameter_values()
		{
			var parser = new MsmqUriParser(new Uri("msmq://./work-queue"));

			Assert.IsTrue(parser.Transactional);
			Assert.IsTrue(parser.Journal);
		}

		[Test]
		public void Should_be_able_to_set_parameter_values()
		{
			var parser = new MsmqUriParser(new Uri("msmq://./work-queue?transactional=false&journal=false"));

			Assert.IsFalse(parser.Transactional);
			Assert.IsFalse(parser.Journal);
		}
	}
}