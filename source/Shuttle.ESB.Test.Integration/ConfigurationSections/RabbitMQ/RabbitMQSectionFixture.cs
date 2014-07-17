using Shuttle.ESB.RabbitMQ;

namespace Shuttle.ESB.Test.Integration
{
	public class RabbitMQSectionFixture
	{
		protected RabbitMQSection GetRabbitMQSection(string file)
		{
			return RabbitMQSection.Open(string.Format(@".\ConfigurationSections\RabbitMQ\files\{0}", file));
		}
	}
}