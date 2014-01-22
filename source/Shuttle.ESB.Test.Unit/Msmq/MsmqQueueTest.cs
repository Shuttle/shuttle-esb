using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Msmq;

namespace Shuttle.ESB.Test.Unit.Msmq
{
	public class MsmqQueueTest : UnitFixture
	{
		[Test]
		public void Should_be_able_to_create_a_new_queue_from_a_given_uri()
		{
			var uri = new Uri("msmq://./inputqueue");
			var configuration = new MsmqConfiguration();

			configuration.AddQueueConfiguration(new MsmqQueueConfiguration(uri, true));

			Assert.AreEqual(string.Format("msmq://{0}/inputqueue", Environment.MachineName.ToLower()), new MsmqQueue(uri, configuration).Uri.ToString());
		}

		[Test]
		public void Should_be_able_to_create_a_new_queue_using_an_ip_address()
		{
			var uri = new Uri("msmq://127.0.0.1/inputqueue");
			var configuration = new MsmqConfiguration();

			configuration.AddQueueConfiguration(new MsmqQueueConfiguration(uri, true));

			Assert.AreEqual("msmq://127.0.0.1/inputqueue", new MsmqQueue(new Uri("msmq://127.0.0.1/inputqueue"), configuration).Uri.ToString());
		}

		[Test]
		public void Should_throw_exception_when_trying_to_create_a_queue_with_incorrect_scheme()
		{
			Assert.Throws<InvalidSchemeException>(() => new MsmqQueue(new Uri("sql://./inputqueue"), new MsmqConfiguration()));
		}
	}
}
