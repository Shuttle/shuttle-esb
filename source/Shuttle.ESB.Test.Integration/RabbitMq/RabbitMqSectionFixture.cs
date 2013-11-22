using Shuttle.ESB.RabbitMq;

namespace Shuttle.ESB.Test.Integration.RabbitMq
{
	public class RabbitMqSectionFixture
	{
		protected RabbitMqSection GetRabbitMqSection(string file)
		{
			return RabbitMqSection.Open(string.Format(@".\RabbitMq\files\{0}", file));
		}
	}
}