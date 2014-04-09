using Shuttle.ESB.Msmq;

namespace Shuttle.ESB.Test.Integration
{
	public class MsmqSectionFixture
	{
		protected MsmqSection GetMsmqSection(string file)
		{
			return MsmqSection.Open(string.Format(@".\ConfigurationSections\Msmq\files\{0}", file));
		}
	}
}