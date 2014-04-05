using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class RabbitMQDeferredMessageTest : DeferredFixture
	{
		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void Should_be_able_to_perform_full_processing(bool isTransactionalEndpoint)
		{
			var queueUriFormat = "rabbitmq://shuttle:shuttle!@localhost/{0}";

			TestDeferredProcessing(queueUriFormat, queueUriFormat, queueUriFormat, isTransactionalEndpoint);
		}
	}
}